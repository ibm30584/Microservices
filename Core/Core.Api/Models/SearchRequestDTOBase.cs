namespace Core.Api.Models
{
    public abstract class SearchRequestDTOBase
    {
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
    }
}
