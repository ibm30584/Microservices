using Core.Application.Models.CQRS;

namespace Core.Api.Models
{
    public class ResponseDTOBase
    {
        public ResultHeader Header { get; set; } = null!;
    }
}
