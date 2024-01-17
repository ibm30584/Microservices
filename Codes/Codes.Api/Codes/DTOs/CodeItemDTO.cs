namespace Codes.Api.Codes.DTOs
{
    public record CodeItemDto(
        int CodeId,
        string Value,
        string Text,
        string? Text2,
        bool Enabled);
}
