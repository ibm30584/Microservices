using Codes.Application.Services.Persistence;
using Codes.Domain.Entities;
using Core.Application.Models;
using Core.Application.Models.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Codes.Application.CodeTypes.Queries.SearchCodeTypes
{
    public class SearchCodeTypesQuery : SearchQueryBase<CodeType>
    {
        public string? Value { get; set; }
        public string? Text { get; set; }

        public class SearchCodeTypesHandler : RequestHandlerBase<SearchCodeTypesQuery, SearchResultBase<CodeType>>
        {
            private readonly ICodesDbContext _codesDbContext;

            public SearchCodeTypesHandler(ICodesDbContext codesDbContext)
            {
                _codesDbContext = codesDbContext;
            }
            public override async Task<SearchResultBase<CodeType>> Handle(SearchCodeTypesQuery request, CancellationToken cancellationToken)
            {
                var query = CreateQuery(request);
                var result = await query
                    .Paginate(request.PageNumber, request.PageSize)
                    .ToArrayAsync(cancellationToken);
                var resultMetadata = await query.GetResultMetadata(request.PageSize, cancellationToken);
                return MapResult(result, resultMetadata);
            }

            private IQueryable<CodeType> CreateQuery(SearchCodeTypesQuery request)
            {
                var baseQuery = _codesDbContext.CodeTypes.AsNoTracking();
                baseQuery = string.IsNullOrWhiteSpace(request.Value)
                    ? baseQuery
                    : baseQuery.Where(x => EF.Functions.Like(x.Value, request.Value + "%"));
                var hasTextFilter = string.IsNullOrWhiteSpace(request.Text);

                if (!hasTextFilter)
                {
                    return baseQuery;
                }

                var queryByText = baseQuery.Where(x => EF.Functions.Like(x.Text, request.Text + "%"));
                var queryByText2 = baseQuery.Where(x => EF.Functions.Like(x.Text2, request.Text + "%"));

                return queryByText.Union(queryByText2);
            }


            private static SearchResultBase<CodeType> MapResult(CodeType[] result, SearchResultMetadata resultMetadata)
            {
                return new SearchResultBase<CodeType>
                {
                    Data = result,
                    Metadata = resultMetadata
                };
            }
        }
    }
}
