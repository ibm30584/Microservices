namespace Codes.Application.CodeTypes.Queries.GetCodeType
{
    public class GetCodeTypeResult
    {
        public required int CodeTypeId { get; set; }
        public required string Value { get; set; } = null!;
        public required string Text { get; set; } = null!;
        public string? Text2 { get; set; }
    }
}