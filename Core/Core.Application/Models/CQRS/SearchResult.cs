namespace Core.Application.Models.CQRS
{
    public class SearchResult<TResultItem>: ResultBase<SearchResultBody<TResultItem>>
    {
    }
    public record SearchResultBody<TResultItem>(SearchResultMetadata Metadata, TResultItem[] Data);
}
