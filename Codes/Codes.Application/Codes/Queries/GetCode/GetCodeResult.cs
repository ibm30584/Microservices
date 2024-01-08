using Codes.Domain.Entities;

namespace Codes.Application.Codes.Queries.GetCode
{
    public class GetCodeResult
    {
        public int Id { get; set; }
        public string Value { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string? Text2 { get; set; }
        public bool Enabled { get; set; } = true;
        public string CodeTypeText { get; set; } = null!;
        public string? CodeTypeText2 { get; set; }
        public List<Metadata> Metadata { get; set; } = [];
    }
}