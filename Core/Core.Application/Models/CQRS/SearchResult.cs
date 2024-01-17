using Core.Application.Enums;

namespace Core.Application.Models.CQRS
{
    public record SearchResult<TResultItem>(
        TResultItem[] Data,
        SearchResultMetadata Metadata);
}
