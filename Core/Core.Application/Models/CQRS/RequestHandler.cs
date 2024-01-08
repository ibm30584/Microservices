using Core.Application.Enums;
using Core.Application.Exceptions;
using MediatR;
using System.Linq.Expressions;

namespace Core.Application.Models.CQRS
{
    public abstract class RequestHandlerCore<TRequest>
    {
        public List<ResultError> Errors { get; set; } = [];

        protected void ThrowBadRequest(Expression<Func<TRequest, object>> target, string errorMessage)
        {
            ThrowBadRequest(GetFieldName(target), errorMessage);
        }
        protected void ThrowBadRequest(string target, string errorMessage)
        {
            throw new BusinessException(errorMessage)
            {
                ErrorCode = AppStatusCode.BadRequest,
                Target = target,
                Errors = Errors?.Count > 0 ? Errors : null
            };
        }

        protected void ThrowNotFound(Expression<Func<TRequest, object>> target, string errorMessage)
        {
            ThrowNotFound(GetFieldName(target), errorMessage);
        }
        protected void ThrowNotFound(string target, string errorMessage)
        {
            throw new BusinessException(errorMessage)
            {
                ErrorCode = AppStatusCode.NotFound,
                Target = target,
                Errors = Errors?.Count > 0 ? Errors : null
            };
        }

        protected void ThrowUnauthorized(Expression<Func<TRequest, object>> target, string errorMessage)
        {
            ThrowUnauthorized(GetFieldName(target), errorMessage);
        }
        protected void ThrowUnauthorized(string target, string errorMessage)
        {
            throw new BusinessException(errorMessage)
            {
                ErrorCode = AppStatusCode.Unauthorized,
                Target = target,
                Errors = Errors?.Count > 0 ? Errors : null
            };
        }

        protected void ThrowForbidden(Expression<Func<TRequest, object>> target, string errorMessage)
        {
            ThrowForbidden(GetFieldName(target), errorMessage);
        }
        protected void ThrowForbidden(string target, string errorMessage)
        {
            throw new BusinessException(errorMessage)
            {
                ErrorCode = AppStatusCode.Forbidden,
                Target = target,
                Errors = Errors?.Count > 0 ? Errors : null
            };
        }

        private static string GetFieldName(Expression<Func<TRequest, object>> field)
        {
            return field.Body is MemberExpression memberExpression
                ? memberExpression.Member.Name
                : field.Body is UnaryExpression unaryExpression &&
                                     unaryExpression.Operand is MemberExpression innerMemberExpression
                    ? innerMemberExpression.Member.Name
                    : throw new ArgumentException("Expression is not a valid member access expression.", nameof(field));
        }
    }

    /// <summary>
    /// Base class for CQRS handlers derived from RequestHandler<TRequest, TResult> to support methods of
    /// ThrowBadRequest, ThrowNotFound, ThrowUnauthorized, ThrowForbidden
    /// </summary>
    public abstract class RequestHandler<TRequest, TResult> : RequestHandlerCore<TRequest>, IRequestHandler<TRequest, TResult>
        where TRequest : IRequest<TResult>
    {
        public abstract Task<TResult> Handle(TRequest request, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Base class for CQRS handlers derived from RequestHandler<TRequest> to support methods of
    /// ThrowBadRequest, ThrowNotFound, ThrowUnauthorized, ThrowForbidden
    /// </summary>
    public abstract class RequestHandler<TRequest> : RequestHandlerCore<TRequest>, IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        public abstract Task Handle(TRequest request, CancellationToken cancellationToken);
    }
}
