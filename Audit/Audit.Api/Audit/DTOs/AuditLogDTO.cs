namespace Codes.Api.Codes.DTOs
{
    public record AuditLogDTO(
        string Description,
        List<Metadata> Metadata);


    public record Metadata(string Key, string Value);
}
