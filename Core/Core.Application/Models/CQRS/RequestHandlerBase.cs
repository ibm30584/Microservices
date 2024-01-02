using Core.Application.Enums;
using MediatR;
using MediatR.Wrappers;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Core.Application.Models.CQRS
{
    public abstract class RequestHandlerBase<TRequest, TResult> : IRequestHandler<TRequest, TResult>
        where TRequest : RequestBase<TResult>
        where TResult : ResultBase, new()
    {
        public string CorrelationId { get; set; } = null!;
        public ClaimsIdentity Claims { get; set; } = null!;
        public AppLanguage Language { get; set; }


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
                    ErrorCode = AppErrorCode.BadRequest,
                    ErrorMessage = errorMessage,
                    Target = target,
                    Errors = Errors?.Count > 0 ? Errors : null
                }
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
                    ErrorCode = AppErrorCode.NotFound,
                    ErrorMessage = errorMessage,
                    Target = target
                }
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
                    ErrorCode = AppErrorCode.Unauthorized,
                    ErrorMessage = errorMessage,
                    Target = target
                }
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
                    ErrorCode = AppErrorCode.Forbidden,
                    ErrorMessage = errorMessage,
                    Target = target
                }
            };
        }

        protected TResult Ok()
        {
            return new TResult()
            {
                Header = new ResultHeader
                {
                    ErrorCode = AppErrorCode.Ok
                }
            };
        }
        protected TResult Ok([NotNull] TResult result)
        {
            result.Header = new ResultHeader
            {
                ErrorCode = AppErrorCode.Ok
            };
            return result;
        }


        private static string GetFieldName(Expression<Func<TRequest, object>> field)
        {
            if (field.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            else if (field.Body is UnaryExpression unaryExpression &&
                     unaryExpression.Operand is MemberExpression innerMemberExpression)
            {
                return innerMemberExpression.Member.Name;
            }
            else
            {
                throw new ArgumentException("Expression is not a valid member access expression.", nameof(field));
            }
        }
    }

    public abstract class RequestHandlerBase<TRequest> : RequestHandlerBase<TRequest, ResultBase>
         where TRequest : RequestBase<ResultBase>
    {

    }
}
