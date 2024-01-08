using Audit.Contracts.Messages;
using Codes.Application.Services.Audit;
using Codes.Application.Services.Persistence;
using Codes.Domain.Entities;
using Core.Application.Models.CQRS;
using Microsoft.EntityFrameworkCore;

namespace Codes.Application.Codes.Commands.AddCode
{
    public class AddCodeCommand : RequestBase<ResultBase<AddCodeResult>>
    {
        public string CodeType { get; set; } = null!;
        public string Value { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string? Text2 { get; set; }
        public bool Enabled { get; set; }
        public List<Metadata>? Metadata { get; set; } = [];

        public class AddCodeHandler : RequestHandlerBase<AddCodeCommand, ResultBase<AddCodeResult>>
        {
            private readonly ICodesDbContext _codesDbContext;
            private readonly IAuditService _auditService;

            public AddCodeHandler(
                ICodesDbContext codesDbContext,
                IAuditService auditService)
            {
                _codesDbContext = codesDbContext;
                _auditService = auditService;
            }
            public override async Task<ResultBase<AddCodeResult>> Handle(AddCodeCommand request, CancellationToken cancellationToken)
            {
                var dbCode = await GetCodeEntity(request.Value, cancellationToken);
                if (dbCode != null)
                {
                    return BadRequest(x => x.Value, "There is an existing code with the same value");
                }

                var codeTypeId = await GetCodeTypeId(request.CodeType, cancellationToken);
                var code = CreateCodeEntity(request, codeTypeId);
                await Persist(code, cancellationToken);

                var auditLogMessage = CreateAuditMessage(code);
                await Audit(auditLogMessage);

                return Ok(new AddCodeResult
                {
                    CodeId = codeTypeId,
                });
            }

            private async Task<Code?> GetCodeEntity(string value, CancellationToken cancellationToken)
            {
                return await _codesDbContext
                    .Codes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Value == value, cancellationToken);
            }

            private async Task<int> GetCodeTypeId(string codeType, CancellationToken cancellationToken)
            {
                return await _codesDbContext
                    .CodeTypes
                    .Where(x => x.Value == codeType)
                    .Select(x => x.CodeTypeId)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            private static Code CreateCodeEntity(AddCodeCommand request, int codeTypeId)
            {
                return new Code
                {
                    CodeTypeId = codeTypeId,
                    Value = request.Value,
                    Text = request.Text,
                    Text2 = request.Text2,
                    Enabled = request.Enabled,
                    Metadata = request.Metadata
                };
            }

            private async Task Persist(Code code, CancellationToken cancellationToken)
            {
                await _codesDbContext.Codes.AddAsync(code, cancellationToken);
                await _codesDbContext.SaveChangesAsync(cancellationToken);
            }


            private static AuditLogMessage CreateAuditMessage(Code code)
            {
                return new AuditLogMessage
                {
                    CreatedDate = code.CreatedDate,
                    CreatedByUserId = code.CreatedByUserId,
                    ServiceId = global::Audit.Contracts.Enums.AuditService.CodesManagement,
                    EventId = global::Audit.Contracts.Enums.AuditEvent.AddCode,
                    EventEntityId = code.CodeId.ToString(),
                    Metadata =
                    [
                        new("Value", code.Value),
                        new("Text", code.Text),
                        new("Text2", code.Text2 ?? string.Empty)
                    ]
                };
            }

            private async Task Audit(AuditLogMessage auditLogMessage)
            {
                await _auditService.Audit(auditLogMessage);
            }
        }
    }
}
