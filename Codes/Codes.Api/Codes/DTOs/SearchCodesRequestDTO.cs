using Core.Api.Models;

namespace Codes.Api.Codes.DTOs
{
    public class SearchCodesRequestDto : SearchRequestDtoBase
    {
        public string? Value { get; set; }
        public string? Text { get; set; }
        public bool? Enabled { get; set; } = true;
    }
}
