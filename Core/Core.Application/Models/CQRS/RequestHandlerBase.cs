using Core.Application.Enums;
using Core.Application.Exceptions;
using MediatR;
using System.Linq.Expressions;

namespace Core.Application.Models.CQRS
{
    /// <summary>
    /// Base class for CQRS handlers derived from RequestHandlerBase<TRequest, TResult> to support methods of
    /// ThrowBadRequest, ThrowNotFound, ThrowUnauthorized, ThrowForbidden,
    /// BadRequest, NotFound, Unauthorized, Forbidden
    /// 
    /// To work with this type of handlers, Request must inherit RequestBase 
    /// and must return Result of type ResultBase<TBody> where TBody is the business result
    /// 
    /// Also final API response of type ResponseDTOBase<TBody> where TBody is the business response dto
    /// </summary>
    public abstract class RequestHandlerBase<TRequest, TResult> : IRequestHandler<TRequest, TResult>
        where TRequest : IRequestExtended<TResult>
        where TResult : ResultBase, new()
    {
        public abstract Task<TResult> Handle(TRequest request, CancellationToken cancellationToken);

        public List<ResultError> Errors { get; set; } = [];

        protected TResult BadRequest(Expression<Func<TRequest, object>> target, string errorMessage)
        {
            return BadRequest(GetFieldName(target), errorMessage);
        }
        protected TResult BadRequest(string target, string errorMessage)
        {
            return new TResult()
            {
                Header = new ResultHeader
                {
                    StatusCode = ResultCode.BadRequest,
                    ErrorMessage = errorMessage,
                    Target = target,
                    Errors = Errors?.Count > 0 ? Errors : null
                }
            };
        }
        protected void ThrowBadRequest(Expression<Func<TRequest, object>> target, string errorMessage)
        {
            ThrowBadRequest(GetFieldName(target), errorMessage);
        }
        protected void ThrowBadRequest(string target, string errorMessage)
        {
            throw new BusinessException(errorMessage)
            {
                ErrorCode = ResultCode.BadRequest,
                Target = target,
                Errors = Errors?.Count > 0 ? Errors : null
            };
        }

        protected TResult NotFound(Expression<Func<TRequest, object>> target, string errorMessage)
        {
            return NotFound(GetFieldName(target), errorMessage);
        }
        protected TResult NotFound(string target, string errorMessage)
        {
            return new TResult()
            {
                Header = new ResultHeader
                {
                    StatusCode = ResultCode.NotFound,
                    ErrorMessage = errorMessage,
                    Target = target
                }
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
                ErrorCode = ResultCode.NotFound,
                Target = target,
                Errors = Errors?.Count > 0 ? Errors : null
            };
        }

        protected TResult Unauthorized(Expression<Func<TRequest, object>> target, string errorMessage)
        {
            return Unauthorized(GetFieldName(target), errorMessage);
        }
        protected TResult Unauthorized(string target, string errorMessage)
        {
            return new TResult()
            {
                Header = new ResultHeader
                {
                    StatusCode = ResultCode.Unauthorized,
                    ErrorMessage = errorMessage,
                    Target = target
                }
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
                ErrorCode = ResultCode.Unauthorized,
                Target = target,
                Errors = Errors?.Count > 0 ? Errors : null
            };
        }

        protected TResult Forbidden(Expression<Func<TRequest, object>> target, string errorMessage)
        {
            return Forbidden(GetFieldName(target), errorMessage);
        }
        protected TResult Forbidden(string target, string errorMessage)
        {
            return new TResult()
            {
                Header = new ResultHeader
                {
                    StatusCode = ResultCode.Forbidden,
                    ErrorMessage = errorMessage,
                    Target = target
                }
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
                ErrorCode = ResultCode.Forbidden,
                Target = target,
                Errors = Errors?.Count > 0 ? Errors : null
            };
        }

        protected TResult Ok()
        {
            return new TResult()
            {
                Header = new ResultHeader
                {
                    StatusCode = ResultCode.Ok
                }
            };
        }

        protected TResult Ok(TResult result)
        {
            result.Header = new ResultHeader
            {
                StatusCode = ResultCode.Ok
            };
            return result;
        }

        protected ResultBase<TBody> Ok<TBody>(TBody? body)
        {
            return new ResultBase<TBody>()
            {
                Header = new ResultHeader
                {
                    StatusCode = ResultCode.Ok
                },
                Body = body
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
    /// Base class for CQRS handlers derived from RequestHandlerBase<TRequest> to support methods of
    /// ThrowBadRequest, ThrowNotFound, ThrowUnauthorized, ThrowForbidden,
    /// BadRequest, NotFound, Unauthorized, Forbidden
    /// 
    /// To work with this type of handlers, Request must inherit RequestBase with no result
    /// 
    /// Also final API response of type ResponseDTOBase with no business response dto
    /// </summary>
    public abstract class RequestHandlerBase<TRequest> : RequestHandlerBase<TRequest, ResultBase>
         where TRequest : IRequestExtended<ResultBase>
    {

    }
}
