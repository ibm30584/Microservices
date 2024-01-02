using Codes.Application.Services.Persistence;
using Codes.Domain.Entities;
using Core.Application.Exceptions;
using Core.Application.Models.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codes.Application.Codes.Commands.EditCode
{

    public class EditCodeCommand : RequestBase
    {
        public int CodeId { get; set; }
        public string Value { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string? Text2 { get; set; }
        public bool Enabled { get; set; }
        public List<Metadata>? Metadata { get; set; }

        public class EditCodeHandler : RequestHandlerBase<EditCodeCommand>
        {
            private readonly ICodesDbContext _codesDbContext;

            public EditCodeHandler(ICodesDbContext codesDbContext)
            {
                _codesDbContext = codesDbContext;
            }
            public override async Task<ResultBase> Handle(EditCodeCommand request, CancellationToken cancellationToken)
            {
                var dbCode = await GetCodeEntity(request.CodeId, cancellationToken);
                if (dbCode == null)
                    return NotFound(x=>x.CodeId, "There is no code stored with provided id");


                var dbOtherCodeWithSameValue = await GetCodeEntity(request.CodeId, request.Value, cancellationToken);
                BusinessException.Must(
                    dbOtherCodeWithSameValue == null,
                    "There is an other code stored with the same value");

                MapUpdate(dbCode, request);
                await Persist(dbCode, cancellationToken);

                return Ok();
            }

            private async Task<Code?> GetCodeEntity(int codeId, CancellationToken cancellationToken)
            {
                return await _codesDbContext
                    .Codes
                    .FirstOrDefaultAsync(x => x.CodeId == codeId, cancellationToken);
            }

            private async Task<Code?> GetCodeEntity(int codeId, string value, CancellationToken cancellationToken)
            {
                return await _codesDbContext
                    .Codes
                    .FirstOrDefaultAsync(
                    x => x.CodeId != codeId && x.Value == value,
                    cancellationToken);
            }

            private static void MapUpdate(Code dbCode, EditCodeCommand request)
            {
                dbCode.Value = request.Value;
                dbCode.Text = request.Text;
                dbCode.Text2 = request.Text2;
                dbCode.Enabled = request.Enabled;
                dbCode.Metadata = request.Metadata;
            }
            private async Task Persist(Code code, CancellationToken cancellationToken)
            {
                await _codesDbContext.Codes.AddAsync(code, cancellationToken);
                await _codesDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
