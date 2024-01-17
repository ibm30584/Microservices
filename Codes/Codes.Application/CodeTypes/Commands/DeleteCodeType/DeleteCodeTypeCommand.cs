using Codes.Application.Services.Persistence;
using Codes.Domain.Entities;
using Core.Application.Models.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codes.Application.CodeTypes.Commands.DeleteCodeType
{
    public class DeleteCodeTypeCommand : IRequest<Result>
    {
        public int CodeTypeId { get; set; }
        public class DeleteCodeTypeHandler : IRequestHandler<DeleteCodeTypeCommand,Result>
        {
            private readonly ICodesDbContext _codesDbContext;

            public DeleteCodeTypeHandler(ICodesDbContext codesDbContext)
            {
                _codesDbContext = codesDbContext;
            }
            public async Task<Result> Handle(DeleteCodeTypeCommand request, CancellationToken cancellationToken)
            {
                var dbCodeType = await GetCodeTypeEntity(request.CodeTypeId, cancellationToken);
                if (dbCodeType == null)
                {
                    return Result.NotFound<DeleteCodeTypeCommand>(x => x.CodeTypeId, "There is no code type stored with provided id");
                }

                await Persist(dbCodeType, cancellationToken);
                return Result.OK;
            }

            private Task<CodeType?> GetCodeTypeEntity(int codeTypeId, CancellationToken cancellationToken)
            {
                return _codesDbContext
                    .CodeTypes
                    .FirstOrDefaultAsync(x => x.CodeTypeId == codeTypeId, cancellationToken);
            }

            private Task<int> Persist(CodeType codeType, CancellationToken cancellationToken)
            {
                _codesDbContext.CodeTypes.Remove(codeType);
                return _codesDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
