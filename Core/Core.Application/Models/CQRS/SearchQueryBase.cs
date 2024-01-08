namespace Core.Application.Models.CQRS
{
    public abstract class SearchQueryBase<TResultItem> : RequestBase<SearchResult<TResultItem>>
        where TResultItem : class
    {
        public required string Language { get; set; }
        public required int? PageNumber { get; set; }
        public required int? PageSize { get; set; }
    }
}
