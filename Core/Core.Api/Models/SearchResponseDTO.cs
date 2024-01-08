using Core.Application.Models.CQRS;

namespace Core.Api.Models
{
    public class SearchResponseDTO<TResultItem> : ResponseDTOBase<SearchResponseBodyDTO<TResultItem>>
    {
       
    }
    public record SearchResponseBodyDTO<TResultItem>(SearchResultMetadata Metadata, TResultItem[] Data);
}
