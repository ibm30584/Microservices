using Core.Api.Models;

namespace Codes.Api.CodeTypes.DTOs
{
    public class SearchCodeTypesRequestDto : SearchRequestDtoBase
    {
        public string? Value { get; set; }
        public string? Text { get; set; }
        public bool? Enabled { get; set; } = true;
    }
}
