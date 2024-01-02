using Core.Application.Enums;
using Core.Application.Models.CQRS;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Core.Api.Models
{
    public class SearchResponseDTOBase<TResultItem> : ResponseDTOBase
    {
        public required SearchResultMetadata Metadata { get; set; }
        public TResultItem[] Result { get; set; } = [];
    }
}
