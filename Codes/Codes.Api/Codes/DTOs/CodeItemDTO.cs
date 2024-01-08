namespace Codes.Api.Codes.DTOs
{
    public record CodeItemDTO(
        int CodeId,
        string Value,
        string Text,
        string? Text2,
        bool Enabled);
}
