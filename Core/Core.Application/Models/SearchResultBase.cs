namespace Core.Application.Models
{
    public class SearchResultBase<TResultItem>
    {
        public required SearchResultMetadata Metadata { get; set; }
        public TResultItem[] Result { get; set; } = [];
    }
}
