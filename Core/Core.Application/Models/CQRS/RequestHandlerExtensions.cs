using Core.Application.Enums;
using Core.Application.Exceptions;
using MediatR;
using System.Linq.Expressions;

namespace Core.Application.Models.CQRS
{
    /// <summary>
    /// Extensions for native CQRS handlers derived from IRequestHandler<TRequest, TResult> to support methods of
    /// ThrowBadRequest, ThrowNotFound, ThrowUnauthorized, ThrowForbidden
    /// </summary>
    public static class RequestHandlerExtensions
    {
        public static BusinessException BadRequest<TRequest, TResult>(this IRequestHandler<TRequest, TResult> handler, Expression<Func<TRequest, object>> target, string errorMessage, List<ResultError>? errors = null)
            where TRequest : IRequest<TResult>
        {
            return handler.BadRequest(GetFieldName<TRequest, TResult>(target), errorMessage, errors);
        }
        public static BusinessException BadRequest<TRequest, TResult>(this IRequestHandler<TRequest, TResult> _, string target, string errorMessage, List<ResultError>? errors = null)
            where TRequest : IRequest<TResult>
        {
            return new BusinessException(errorMessage)
            {
                ErrorCode = AppStatusCode.BadRequest,
                Target = target,
                Errors = errors?.Count > 0 ? errors : null
            };
        }
        public static BusinessException BadRequest<TRequest>(this IRequestHandler<TRequest> handler, Expression<Func<TRequest, object>> target, string errorMessage, List<ResultError>? errors = null)
            where TRequest : IRequest
        {
            return handler.BadRequest(GetFieldName(target), errorMessage, errors);
        }
        public static BusinessException BadRequest<TRequest>(this IRequestHandler<TRequest> _, string target, string errorMessage, List<ResultError>? errors = null)
            where TRequest : IRequest
        {
            return new BusinessException(errorMessage)
            {
                ErrorCode = AppStatusCode.BadRequest,
                Target = target,
                Errors = errors?.Count > 0 ? errors : null
            };
        }

        public static BusinessException NotFound<TRequest, TResult>(this IRequestHandler<TRequest, TResult> handler, Expression<Func<TRequest, object>> target, string errorMessage, List<ResultError>? errors = null)
            where TRequest : IRequest<TResult>
        {
            return handler.NotFound(GetFieldName<TRequest, TResult>(target), errorMessage, errors);
        }
        public static BusinessException NotFound<TRequest, TResult>(this IRequestHandler<TRequest, TResult> _, string target, string errorMessage, List<ResultError>? errors = null)
            where TRequest : IRequest<TResult>
        {
            return new BusinessException(errorMessage)
            {
                ErrorCode = AppStatusCode.NotFound,
                Target = target,
                Errors = errors?.Count > 0 ? errors : null
            };
        }
        public static BusinessException NotFound<TRequest>(this IRequestHandler<TRequest> handler, Expression<Func<TRequest, object>> target, string errorMessage, List<ResultError>? errors = null)
         where TRequest : IRequest
        {
            return handler.NotFound(GetFieldName(target), errorMessage, errors);
        }
        public static BusinessException NotFound<TRequest>(this IRequestHandler<TRequest> _, string target, string errorMessage, List<ResultError>? errors = null)
            where TRequest : IRequest
        {
            return new BusinessException(errorMessage)
            {
                ErrorCode = AppStatusCode.NotFound,
                Target = target,
                Errors = errors?.Count > 0 ? errors : null
            };
        }

        public static BusinessException Unauthorized<TRequest, TResult>(this IRequestHandler<TRequest, TResult> handler, Expression<Func<TRequest, object>> target, string errorMessage, List<ResultError>? errors = null)
            where TRequest : IRequest<TResult>
        {
            return handler.Unauthorized(GetFieldName<TRequest, TResult>(target), errorMessage, errors);
        }
        public static BusinessException Unauthorized<TRequest, TResult>(this IRequestHandler<TRequest, TResult> _, string target, string errorMessage, List<ResultError>? errors = null)
            where TRequest : IRequest<TResult>
        {
            return new BusinessException(errorMessage)
            {
                ErrorCode = AppStatusCode.Unauthorized,
                Target = target,
                Errors = errors?.Count > 0 ? errors : null
            };
        }
        public static BusinessException Unauthorized<TRequest>(this IRequestHandler<TRequest> handler, Expression<Func<TRequest, object>> target, string errorMessage, List<ResultError>? errors = null)
           where TRequest : IRequest
        {
            return handler.Unauthorized(GetFieldName(target), errorMessage, errors);
        }
        public static BusinessException Unauthorized<TRequest>(this IRequestHandler<TRequest> _, string target, string errorMessage, List<ResultError>? errors = null)
            where TRequest : IRequest
        {
            return new BusinessException(errorMessage)
            {
                ErrorCode = AppStatusCode.Unauthorized,
                Target = target,
                Errors = errors?.Count > 0 ? errors : null
            };
        }

        public static BusinessException Forbidden<TRequest, TResult>(this IRequestHandler<TRequest, TResult> handler, Expression<Func<TRequest, object>> target, string errorMessage, List<ResultError>? errors = null)
             where TRequest : IRequest<TResult>
        {
            return handler.Forbidden(GetFieldName<TRequest, TResult>(target), errorMessage, errors);
        }
        public static BusinessException Forbidden<TRequest, TResult>(this IRequestHandler<TRequest, TResult> _, string target, string errorMessage, List<ResultError>? errors = null)
            where TRequest : IRequest<TResult>
        {
            return new BusinessException(errorMessage)
            {
                ErrorCode = AppStatusCode.Forbidden,
                Target = target,
                Errors = errors?.Count > 0 ? errors : null
            };
        }
        public static BusinessException Forbidden<TRequest>(this IRequestHandler<TRequest> handler, Expression<Func<TRequest, object>> target, string errorMessage, List<ResultError>? errors = null)
         where TRequest : IRequest
        {
            return handler.Forbidden(GetFieldName(target), errorMessage, errors);
        }
        public static BusinessException Forbidden<TRequest>(this IRequestHandler<TRequest> _, string target, string errorMessage, List<ResultError>? errors = null)
            where TRequest : IRequest
        {
            return new BusinessException(errorMessage)
            {
                ErrorCode = AppStatusCode.Forbidden,
                Target = target,
                Errors = errors?.Count > 0 ? errors : null
            };
        }

        private static string GetFieldName<TRequest, TResult>(Expression<Func<TRequest, object>> field)
            where TRequest : IRequest<TResult>
        {
            return field.Body is MemberExpression memberExpression
                ? memberExpression.Member.Name
                : field.Body is UnaryExpression unaryExpression &&
                                     unaryExpression.Operand is MemberExpression innerMemberExpression
                    ? innerMemberExpression.Member.Name
                    : throw new ArgumentException("Expression is not a valid member access expression.", nameof(field));
        }
        private static string GetFieldName<TRequest>(Expression<Func<TRequest, object>> field)
               where TRequest : IRequest
        {
            return field.Body is MemberExpression memberExpression
                ? memberExpression.Member.Name
                : field.Body is UnaryExpression unaryExpression &&
                                     unaryExpression.Operand is MemberExpression innerMemberExpression
                    ? innerMemberExpression.Member.Name
                    : throw new ArgumentException("Expression is not a valid member access expression.", nameof(field));
        }
    }
}
