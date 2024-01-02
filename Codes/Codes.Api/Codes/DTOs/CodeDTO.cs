namespace Codes.Api.Codes.DTOs
{
    public record CodeDTO(
        string Value,
        string Text,
        string? Text2,
        bool Enabled,
        List<Metadata> Metadata);


    public record Metadata(string Key, string Value);
}
