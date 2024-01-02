using MediatR;

namespace Core.Application.Models.CQRS
{
    public abstract class RequestBase<TResponseBase> : IRequest<TResponseBase>
        where TResponseBase : ResultBase
    {
        public RequestHeader Header { get; set; } = null!;
    }

    public abstract class RequestBase : RequestBase<ResultBase>
    {
    }
}
