namespace Codes.Application.CodeTypes.Queries.GetCodeType
{
    public class GetCodeTypeResult
    {
        public int CodeTypeId { get; set; }
        public string Value { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string? Text2 { get; set; }
    }
}