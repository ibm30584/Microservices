using Codes.Application.Services.Persistence;
using Codes.Domain.Entities;
using Core.Application.Models.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codes.Application.Codes.Queries.GetCode
{
    public class GetCodeQuery : IRequest<Result<GetCodeResult>>
    {
        public int CodeID { get; set; }
        public class GetCodeHandler : IRequestHandler<GetCodeQuery, Result<GetCodeResult>>
        {
            private readonly ICodesDbContext _codesDbContext;

            public GetCodeHandler(ICodesDbContext codesDbContext)
            {
                _codesDbContext = codesDbContext;
            }
            public async Task<Result<GetCodeResult>> Handle(GetCodeQuery request, CancellationToken cancellationToken)
            {
                var dbCode = await GetCodeEntity(request.CodeID, cancellationToken);
                return dbCode == null
                    ? Result<GetCodeResult>.NotFound<GetCodeQuery>(x => x.CodeID, "There is no code stored with provide id")
                    : Result.Ok(MapResponse(dbCode));
            }

            private async Task<Code?> GetCodeEntity(int codeId, CancellationToken cancellationToken)
            {
                return await _codesDbContext
                    .Codes
                    .Include(x => x.CodeType)
                    .FirstOrDefaultAsync(x => x.CodeId == codeId, cancellationToken);
            }

            private static GetCodeResult MapResponse(Code dbCode)
            {
                return new GetCodeResult
                {
                    Id = dbCode.CodeId,
                    Value = dbCode.Value,
                    Text = dbCode.Text,
                    Text2 = dbCode.Text2,
                    Enabled = dbCode.Enabled,
                    CodeTypeText = dbCode.CodeType.Text,
                    CodeTypeText2 = dbCode.CodeType.Text2,
                    Metadata = [.. dbCode.Metadata]
                };
            }
        }
    }
}
