using MediatR;

namespace Core.Application.Models
{
    public abstract class SearchQueryBase<TResultItem> : IRequest<SearchResultBase<TResultItem>>
        where TResultItem : class
    {
        public required string Language { get; set; }
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
    }
}
