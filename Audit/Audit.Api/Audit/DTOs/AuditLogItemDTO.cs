namespace Audit.Api.Audit.DTOs
{
    public record AuditLogItemDTO(
        int AuditLogId,
        DateTime CreatedDate,
        string CreatedByUserId,
        string ServiceText,
        string EventText,
        string EventEntityId);
}
