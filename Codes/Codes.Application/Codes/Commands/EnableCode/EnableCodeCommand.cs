using Codes.Application.Services.Persistence;
using Codes.Domain.Entities;
using Core.Application.Exceptions;
using Core.Application.Models.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codes.Application.Codes.Commands.EnableCode
{
    public class EnableCodeCommand : RequestBase
    {
        public int Id { get; set; }
        public bool Enabled { get; set; }

        public class EnableCodeHandler : RequestHandlerBase<EnableCodeCommand>
        {
            private readonly ICodesDbContext _codesDbContext;

            public EnableCodeHandler(ICodesDbContext codesDbContext)
            {
                _codesDbContext = codesDbContext;
            }
            public override async Task<ResultBase> Handle(EnableCodeCommand request, CancellationToken cancellationToken)
            {
                var dbCode = await GetCodeEntity(request.Id, cancellationToken);
                if (dbCode == null)
                    return NotFound(x => x.Id, "There is no code stored with provided id");

                MapUpdate(dbCode, request);
                await Persist(dbCode);

                return Ok();
            }

            private async Task Persist(Code dbCode)
            {
                _codesDbContext.Codes.Update(dbCode);
                await _codesDbContext.SaveChangesAsync();
            }

            private Task<Code?> GetCodeEntity(int codeId, CancellationToken cancellationToken)
            {
                return _codesDbContext
                    .Codes
                    .FirstOrDefaultAsync(x => x.CodeId == codeId, cancellationToken);

            }

            private static void MapUpdate(Code dbCode, EnableCodeCommand request)
            {
                dbCode.Enabled = request.Enabled;
            }
        }
    }
}
