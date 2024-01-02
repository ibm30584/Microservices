using Core.Api.Models;

namespace Codes.Api.Codes.DTOs
{
    public class CodeResponseDTO : ResponseDTOBase
    {
        public string Value { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string? Text2 { get; set; }
        public bool Enabled { get; set; }
        public List<CodeMetadataDTO>? Metadata { get; set; }
    }
}
