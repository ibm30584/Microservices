namespace Codes.Api.Codes.DTOs
{
    public record CodeRequestDTO(
        string Value,
        string Text,
        string? Text2,
        bool Enabled,
        List<CodeMetadataDTO>? Metadata);
}
