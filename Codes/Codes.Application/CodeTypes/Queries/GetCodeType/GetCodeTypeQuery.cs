using Codes.Application.Services.Persistence;
using Codes.Domain.Entities;
using Core.Application.Models.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Codes.Application.CodeTypes.Queries.GetCodeType
{
    public class GetCodeTypeQuery : RequestBase<GetCodeTypeResult>
    {
        public int CodeTypeId { get; set; }

        public class GetCodeTypeHandler : RequestHandlerBase<GetCodeTypeQuery, GetCodeTypeResult>
        {
            private readonly ICodesDbContext _codesDbContext;

            public GetCodeTypeHandler(ICodesDbContext codesDbContext)
            {
                _codesDbContext = codesDbContext;
            }
            public override async Task<GetCodeTypeResult> Handle(GetCodeTypeQuery request, CancellationToken cancellationToken)
            {
                var dbCodeType = await GetCodeTypeEntity(request, cancellationToken);
                return dbCodeType == null
                    ? NotFound(x => x.CodeTypeId, "This is no code type stored with provided id")
                    : Ok(MapResult(dbCodeType));
            }

            private Task<CodeType?> GetCodeTypeEntity(GetCodeTypeQuery request, CancellationToken cancellationToken)
            {
                return _codesDbContext
                    .CodeTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.CodeTypeId == request.CodeTypeId, cancellationToken);
            }

            private static GetCodeTypeResult MapResult(CodeType dbCodeType)
            {
                return new GetCodeTypeResult
                {
                    CodeTypeId = dbCodeType.CodeTypeId,
                    Value = dbCodeType.Value,
                    Text = dbCodeType.Text,
                    Text2 = dbCodeType.Text2
                };
            }
        }
    }
}
