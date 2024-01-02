namespace Core.Application.Models.CQRS
{
    public class SearchResultMetadata
    {
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }

        private SearchResultMetadata(int totalRecords, int totalPages)
        {
            TotalRecords = totalRecords;
            TotalPages = totalPages;
        }
        public static SearchResultMetadata Create(int totalRecords, int pageSize)
        {
            var totalPages = (int)Math.Ceiling((float)totalRecords / pageSize);
            return new SearchResultMetadata(totalRecords, totalPages);
        }
    }
}
