namespace Codes.Api.Codes.DTOs
{
    public record CodeItemDTO(
        int Id,
        string Value,
        string Text,
        string? Text2,
        bool Enabled);
}
