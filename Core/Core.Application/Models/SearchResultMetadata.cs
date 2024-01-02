namespace Core.Application.Models
{
    public class SearchResultMetadata
    {
        private SearchResultMetadata()
        {

        }
        public static SearchResultMetadata Create(int totalRecords, int pageSize)
        {
            var totalPages = (int)Math.Ceiling((float)totalRecords / pageSize);
            return new SearchResultMetadata()
            {
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
    }

}
