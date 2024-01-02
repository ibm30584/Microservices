using Core.Application.Enums;
using System.Security.Claims;

namespace Core.Application.Models.CQRS
{
    public class RequestHeader
    {
        public string CorrelationId { get; set; } = null!;
        public ClaimsPrincipal User { get; set; } = null!;
        public AppLanguage Language { get; set; }
    }
}
