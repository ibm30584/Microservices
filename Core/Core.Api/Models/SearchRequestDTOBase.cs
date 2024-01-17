using Core.Application.Models;

namespace Core.Api.Models
{
    public abstract class SearchRequestDtoBase
    {
        public required string Language { get; set; } = AppConstants.DefaultLanguage;
        public required int? PageNumber { get; set; } = AppConstants.PageNumber; 
        public required int? PageSize { get; set; } = AppConstants.PageSize;
    }
}
