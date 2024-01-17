namespace Codes.Api.Codes.DTOs
{
    public record CodeRequestDto(
        string Value,
        string Text,
        string? Text2,
        bool Enabled,
        List<CodeMetadataDto>? Metadata);
}
