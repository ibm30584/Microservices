using Audit.Contracts.Messages;

namespace Codes.Application.Services.Audit
{
    public interface IAuditService
    {
        Task Audit(AuditLogMessage auditLogMessage);
    }
}
