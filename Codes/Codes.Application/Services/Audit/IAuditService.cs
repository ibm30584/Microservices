namespace Codes.Application.Services.Audit
{
    public interface IAuditService
    {
        Task Audit(AuditLog auditLog);
    }
}
