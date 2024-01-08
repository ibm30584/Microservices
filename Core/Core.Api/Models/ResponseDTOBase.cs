using Core.Application.Models.CQRS;

namespace Core.Api.Models
{
    public sealed class ResponseDTOBase
    {
        public ResultHeader Header { get; set; } = null!;
    }

    public class ResponseDTOBase<TBody>
    {
        public ResultHeader Header { get; set; } = null!;
        public TBody? Body { get; set; }
    }
}
