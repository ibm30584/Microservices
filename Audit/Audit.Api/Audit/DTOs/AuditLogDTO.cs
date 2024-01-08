namespace Audit.Api.Audit.DTOs
{
    public record AuditLogDTO(
        int AuditLogId,
        DateTime CreatedDate,
        string CreatedByUserId ,
        string ServiceText ,
        string EventText,
        string EventEntityId,
        List<AuditLogMetadata>? Metadata);


    public record AuditLogMetadata(string Key, string Value);
}
