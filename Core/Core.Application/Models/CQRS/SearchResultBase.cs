namespace Core.Application.Models.CQRS
{
    public class SearchResultBase<TResultItem>: ResultBase
    {
        public SearchResultMetadata Metadata { get; set; } = null!;
        public TResultItem[] Data { get; set; } = [];
    }
}
