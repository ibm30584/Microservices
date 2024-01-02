using Codes.Application.Services.Persistence;
using Codes.Domain.Entities;
using Core.Application.Models;
using Core.Application.Models.CQRS;
using Microsoft.EntityFrameworkCore;


namespace Codes.Application.Codes.Queries.SearchCodes
{
    public class SearchCodesQuery : SearchQueryBase<Code>
    {
        public required string CodeTypeValue { get; set; }
        public string? Value { get; set; }
        public string? Text { get; set; }
        public bool? Enabled { get; set; } = true;

        public class SearchCodesHandler : RequestHandlerBase<SearchCodesQuery, SearchResultBase<Code>>
        {
            private readonly ICodesDbContext _codesDbContext;

            public SearchCodesHandler(ICodesDbContext codesDbContext)
            {
                _codesDbContext = codesDbContext;
            }
            public override async Task<SearchResultBase<Code>> Handle(SearchCodesQuery request, CancellationToken cancellationToken)
            {
                var dbCodeType = await GetCodeType(request.CodeTypeValue, cancellationToken);
                if (dbCodeType == null)
                {
                    return NotFound(x => x.CodeTypeValue, "There is no code type stored with provided value");
                }

                var query = CreateQuery(request, dbCodeType.CodeTypeId);
                var result = await query
                    .Paginate(request.PageNumber, request.PageSize)
                    .ToArrayAsync(cancellationToken);
                var resultMetadata = await query.GetResultMetadata(request.PageSize, cancellationToken);
                return Ok(MapResult(result, resultMetadata));
            }


            private async Task<CodeType?> GetCodeType(string codeTypeValue, CancellationToken cancellationToken)
            {
                return await _codesDbContext
                    .CodeTypes
                    .Where(x => x.Value == codeTypeValue)
                    .Select(x => new CodeType { CodeTypeId = x.CodeTypeId })
                    .FirstOrDefaultAsync(cancellationToken);
            }

            private IQueryable<Code> CreateQuery(SearchCodesQuery request, int codeTypeId)
            {
                var baseQuery = _codesDbContext
                    .Codes
                    .Where(x => x.CodeTypeId == codeTypeId)
                    .Select(x => new Code
                    {
                        CodeId = x.CodeId,
                        Value = x.Value,
                        Text = x.Text,
                        Text2 = x.Text2,
                        Enabled = x.Enabled
                    });
                baseQuery = string.IsNullOrWhiteSpace(request.Value)
                    ? baseQuery
                    : baseQuery.Where(x => EF.Functions.Like(x.Value, request.Value + "%"));
                baseQuery = request.Enabled.HasValue
                    ? baseQuery
                    : baseQuery.Where(x => x.Enabled == request.Enabled!.Value);

                var queryByText = string.IsNullOrWhiteSpace(request.Text)
                    ? baseQuery
                    : baseQuery.Where(x => EF.Functions.Like(x.Text, request.Text + "%"));

                var queryByText2 = string.IsNullOrWhiteSpace(request.Text)
                    ? baseQuery
                    : baseQuery.Where(x => EF.Functions.Like(x.Text2, request.Text + "%"));

                return queryByText.Union(queryByText2);
            }

            private static SearchResultBase<Code> MapResult(Code[] result, SearchResultMetadata searchResultMetadata)
            {
                return new SearchResultBase<Code>()
                {
                    Metadata = searchResultMetadata,
                    Data = result
                };
            }
        }
    }
}
