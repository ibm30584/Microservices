using Audit.Contracts.Messages;
using Codes.Application.Services.Audit;
using Codes.Application.Services.Persistence;
using Codes.Domain.Entities;
using Core.Application.Models.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Codes.Application.CodeTypes.Commands.AddCodeType3
{
    public class AddCodeTypeCommand3 : IRequest<Result<AddCodeTypeResult3>>
    {
        public required string Value { get; set; }
        public required string Text { get; set; }
        public string? Text2 { get; set; }

        public class AddCodeTypeHandler : IRequestHandler<AddCodeTypeCommand3, Result<AddCodeTypeResult3>>
        {
            private readonly ICodesDbContext _codesDbContext;
            private readonly IAuditService _auditService;

            public AddCodeTypeHandler(
                ICodesDbContext codesDbContext,
                IAuditService auditService)
            {
                _codesDbContext = codesDbContext;
                _auditService = auditService;
            }
            public async Task<Result<AddCodeTypeResult3>> Handle(AddCodeTypeCommand3 request, CancellationToken cancellationToken)
            {
                var dbCodeType = await GetCodeTypeEntity(request.Value, cancellationToken);
                if (dbCodeType != null)
                {
                    return Result<AddCodeTypeResult3>.BadRequest<AddCodeTypeCommand3>(x=>x.Value, "There is a code type stored with the same provided value.");
                } 
                var codeType = CreateCodeTypeEntity(request);
                await Persist(codeType, cancellationToken);

                var auditMessage = CreateAuditMessage(codeType);
                await _auditService.Audit(auditMessage);

                return Result.Ok(new AddCodeTypeResult3() { CodeTypeId = codeType.CodeTypeId });
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
            private static CodeType CreateCodeTypeEntity(AddCodeTypeCommand3 request)
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
            private AuditLogMessage CreateAuditMessage(CodeType codeType)
            {
                return new AuditLogMessage
                {
                    CreatedDate = codeType.CreatedDate,
                    CreatedByUserId = codeType.CreatedByUserId,
                    ServiceId = Audit.Contracts.Enums.AuditService.CodesManagement,
                    EventId = Audit.Contracts.Enums.AuditEvent.AddCodeType,
                    EventEntityId = codeType.CodeTypeId.ToString(),
                    Metadata = [
                        new AuditMetadata("Value", codeType.Value ?? string.Empty),
                        new AuditMetadata("Text", codeType.Text),
                        new AuditMetadata("Text2", codeType.Text2 ?? string.Empty),
                    ]
                };
            }
        }
    }
}
