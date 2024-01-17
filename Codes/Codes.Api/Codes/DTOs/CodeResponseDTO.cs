namespace Codes.Api.Codes.DTOs
{
    public class CodeResponseDto
    {
        public string Value { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string? Text2 { get; set; }
        public bool Enabled { get; set; }
        public List<CodeMetadataDto>? Metadata { get; set; }
    }
}
