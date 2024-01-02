using Core.Api.Models;

namespace Codes.Api.Codes.DTOs
{
    public class SearchCodesRequestDTO : SearchRequestDTOBase
    {
        public string? Value { get; set; }
        public string? Text { get; set; }
        public bool? Enabled { get; set; } = true;
    }
}
