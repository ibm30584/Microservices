namespace Codes.Api.Codes.DTOs
{
    public class CodeResponseDTO
    {
        public string Value { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string? Text2 { get; set; }
        public bool Enabled { get; set; }
        public List<CodeMetadataDTO>? Metadata { get; set; }
    }
}
