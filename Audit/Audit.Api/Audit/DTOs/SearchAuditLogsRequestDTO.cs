using Core.Api.Models;

namespace Codes.Api.Codes.DTOs
{
    public class SearchAuditLogsRequestDTO : SearchRequestDTOBase
    {
        public string? Description { get; set; }
    }
}
