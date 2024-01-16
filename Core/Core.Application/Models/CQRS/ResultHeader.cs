using Core.Application.Enums;

namespace Core.Application.Models.CQRS
{
    public class ResultHeader
    {
        public ResultCode StatusCode { get; set; } = ResultCode.Ok;
        public string? ErrorMessage { get; set; }
        public string? Target { get; set; }
        public List<ResultError>? Errors { get; set; }
    }
}
