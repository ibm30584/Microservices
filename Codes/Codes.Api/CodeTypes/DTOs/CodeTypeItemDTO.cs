namespace Codes.Api.CodeTypes.DTOs
{
    public record CodeTypeItemDto(
        int CodeTypeId,
        string Value,
        string Text,
        string? Text2);
}
