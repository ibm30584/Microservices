using Codes.Domain.Entities;

namespace Codes.Application.Codes.Queries.GetCode
{
    public class GetCodeResult
    {
        public int Id { get; set; }
        public required string Value { get; set; }
        public required string Text { get; set; }
        public string? Text2 { get; set; }
        public bool Enabled { get; set; } = true;
        public required string CodeTypeText { get; set; }
        public string? CodeTypeText2 { get; set; }
        public List<Metadata> Metadata { get; set; } = [];
    }
}