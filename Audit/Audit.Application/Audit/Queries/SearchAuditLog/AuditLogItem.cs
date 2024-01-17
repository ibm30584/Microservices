namespace Audit.Application.Audit.Queries.SearchAuditLog
{
    public record AuditLogItem(
        int AuditLogId,
        DateTime CreatedDate,
        string CreatedByUserId,
        string ServiceText,
        string EventText,
        string EventEntityId);
}
