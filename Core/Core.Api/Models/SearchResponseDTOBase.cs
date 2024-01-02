using Core.Application.Models;

namespace Core.Api.Models
{
    public class SearchResponseDTOBase<TResultItem>
    {
        public required SearchResultMetadata Metadata { get; set; }
        public TResultItem[] Result { get; set; } = [];
    }
}
