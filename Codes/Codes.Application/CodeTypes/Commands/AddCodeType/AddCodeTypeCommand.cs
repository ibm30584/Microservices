using Codes.Application.Services.Persistence;
using Codes.Domain.Entities;
using Core.Application.Models.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Codes.Application.CodeTypes.Commands.AddCodeType
{
    public class AddCodeTypeCommand : RequestBase<AddCodeTypeResult>
    {
        public required string Value { get; set; }
        public required string Text { get; set; }
        public string? Text2 { get; set; }

        public class AddCodeTypeHandler : RequestHandlerBase<AddCodeTypeCommand, AddCodeTypeResult>
        {
            private readonly ICodesDbContext _codesDbContext;

            public AddCodeTypeHandler(ICodesDbContext codesDbContext)
            {
                _codesDbContext = codesDbContext;
            }
            public async override Task<AddCodeTypeResult> Handle(AddCodeTypeCommand request, CancellationToken cancellationToken)
            {
                var dbCodeType = await GetCodeTypeEntity(request.Value, cancellationToken);
                if (dbCodeType != null)
                {
                    return BadRequest(x => x.Value, "There is a code type stored with the same provided value.");
                }

                var codeType = CreateCodeTypeEntity(request);
                await Persist(codeType, cancellationToken);

                return Ok(new AddCodeTypeResult() { CodeTypeId = codeType.CodeTypeId });
            }

            private async Task<CodeType?> GetCodeTypeEntity(string value, CancellationToken cancellationToken)
            {
                return await _codesDbContext
                    .CodeTypes
                    .AsNoTracking()
                    .Where(x => x.Value == value)
                    .Select(x => new CodeType { CodeTypeId = x.CodeTypeId })
                    .FirstOrDefaultAsync(cancellationToken);
            }
            private static CodeType CreateCodeTypeEntity(AddCodeTypeCommand request)
            {
                return new CodeType
                {
                    Value = request.Value,
                    Text = request.Text,
                    Text2 = request.Text2
                };
            }

            private async Task Persist(CodeType codeType, CancellationToken cancellationToken)
            {
                await _codesDbContext.CodeTypes.AddRangeAsync(codeType);
                await _codesDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
