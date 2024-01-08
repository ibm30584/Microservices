using MediatR;

namespace Core.Application.Models.CQRS
{
    public interface IRequestExtended
    {
        RequestHeader Header { get; set; }
    }
    public interface IRequestExtended<TResult> : IRequestExtended, IRequest<TResult>
    {
    }

    public abstract class RequestBase : IRequestExtended<ResultBase>
    {
        public RequestHeader Header { get; set; } = null!;
    }

    public abstract class RequestBase<TResponseBase> : IRequestExtended<TResponseBase>
        where TResponseBase : ResultBase
    {
        public RequestHeader Header { get; set; } = null!;
    }
}
