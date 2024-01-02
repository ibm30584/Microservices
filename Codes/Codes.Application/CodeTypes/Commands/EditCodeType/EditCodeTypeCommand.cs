using Codes.Application.Services.Persistence;
using Codes.Domain.Entities;
using Core.Application.Models.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Codes.Application.CodeTypes.Commands.EditCodeType
{
    public class EditCodeTypeCommand : RequestBase
    {
        public required int CodeTypeId { get; set; }
        public required string Value { get; set; } = null!;
        public required string Text { get; set; } = null!;
        public string? Text2 { get; set; }

        public class EditCodeTypeHandler : RequestHandlerBase<EditCodeTypeCommand>
        {
            private readonly ICodesDbContext _codesDbContext;

            public EditCodeTypeHandler(ICodesDbContext codesDbContext)
            {
                _codesDbContext = codesDbContext;
            }
            public override async Task<ResultBase> Handle(EditCodeTypeCommand request, CancellationToken cancellationToken)
            {
                var dbCodeType = await GetExistingCodeTypeEntity(request.CodeTypeId, cancellationToken);
                if (dbCodeType == null)
                {
                    return NotFound(x => x.CodeTypeId, "There is no code type stored with provide id");
                }

                var dbOtherCodeTypeWithSameValue = await GetExistingCodeTypeEntity(request.Value, cancellationToken);
                if (dbOtherCodeTypeWithSameValue != null)
                {
                    return BadRequest(x => x.Value, "There is an other code type stored with the same value");
                }

                MapUpdate(dbCodeType, request);
                await Persist(dbCodeType, cancellationToken);
                return Ok();
            }

            private async Task<CodeType?> GetExistingCodeTypeEntity(int codeTypeId, CancellationToken cancellationToken)
            {
                return await _codesDbContext
                    .CodeTypes
                    .FirstOrDefaultAsync(x => x.CodeTypeId == codeTypeId, cancellationToken);
            }


            private async Task<CodeType?> GetExistingCodeTypeEntity(string value, CancellationToken cancellationToken)
            {
                return await _codesDbContext
                    .CodeTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Value == value, cancellationToken);
            }

            private static void MapUpdate(CodeType dbCodeType, EditCodeTypeCommand request)
            {
                dbCodeType.Value = request.Value;
                dbCodeType.Text = request.Text;
                dbCodeType.Text2 = request.Text2;
            }

            private async Task Persist(CodeType codeType, CancellationToken cancellationToken)
            {
                _codesDbContext.CodeTypes.Update(codeType);
                await _codesDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
