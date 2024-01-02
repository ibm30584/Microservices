using Codes.Application.Services.Persistence;
using Codes.Domain.Entities;
using Core.Application.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codes.Application.CodeTypes.Queries.GetCodeType
{
    public class GetCodeTypeQuery : IRequest<GetCodeTypeResult>
    {
        public int CodeTypeId { get; set; }

        public class GetCodeTypeHandler : IRequestHandler<GetCodeTypeQuery, GetCodeTypeResult>
        {
            private readonly ICodesDbContext _codesDbContext;

            public GetCodeTypeHandler(ICodesDbContext codesDbContext)
            {
                _codesDbContext = codesDbContext;
            }
            public async Task<GetCodeTypeResult> Handle(GetCodeTypeQuery request, CancellationToken cancellationToken)
            {
                var dbCodeType = await GetCodeTypeEntity(request, cancellationToken);
                BusinessException.ThrowIfNullAsNotFound(dbCodeType, "This is no code type stored with provided id");

                var result = MapResult(dbCodeType);
                return result;
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
