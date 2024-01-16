using Core.Application.Enums;
using System.Linq.Expressions;

namespace Core.Application.Models
{
    public class Result
    {
        internal Result()
        {
        }
        public ResultCode Status { get; set; } = ResultCode.Ok;
        public string? ErrorMessage { get; set; }
        public string? Target { get; set; }
        public List<Error>? Errors { get; set; }


        public static readonly Result OK = new() { Status = ResultCode.Ok };
        public static Result<TValue> Ok<TValue>(TValue value)
        {
            return new Result<TValue>() { Status = ResultCode.Ok, Value = value };
        }

        public static Result BadRequest<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.BadRequest, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static Result BadRequest(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.BadRequest, target, errorMessage, errors);
        }


        public static Result Unauthorized<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.Unauthorized, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static Result Unauthorized(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.Unauthorized, target, errorMessage, errors);
        }


        public static Result Forbidden<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.Forbidden, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static Result Forbidden(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.Forbidden, target, errorMessage, errors);
        }

        public static Result NotFound<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.NotFound, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static Result NotFound(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.NotFound, target, errorMessage, errors);
        }

        public static Result InternalServerError<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.InternalServerError, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static Result InternalServerError(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.InternalServerError, target, errorMessage, errors);
        }

        public static Result BadGateway<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.BadGateway, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static Result BadGateway(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.BadGateway, target, errorMessage, errors);
        }

        public static Result ServiceUnavailable<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.ServiceUnavailable, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static Result ServiceUnavailable(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.ServiceUnavailable, target, errorMessage, errors);
        }


        private static Result GetResult(ResultCode resultCode, string? target, string? errorMessage, List<Error>? errors)
        {
            return new Result() { Status = resultCode, Target = target, ErrorMessage = errorMessage, Errors = errors };
        }
    }

    public class Result<TValue> : Result
    {
        internal Result()
        {
        }

        public TValue? Value { get; set; }

        public static Result<TValue> Ok(TValue value)
        {
            return new Result<TValue>() { Status = ResultCode.Ok, Value = value };
        }

        public static new Result<TValue> BadRequest<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.BadRequest, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static new Result<TValue> BadRequest(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.BadRequest, target, errorMessage, errors);
        }

        public static new Result<TValue> Unauthorized<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.Unauthorized, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static new Result<TValue> Unauthorized(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.Unauthorized, target, errorMessage, errors);
        }

        public static new Result<TValue> Forbidden<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.Forbidden, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static new Result<TValue> Forbidden(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.Forbidden, target, errorMessage, errors);
        }

        public static new Result<TValue> NotFound<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.NotFound, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static new Result<TValue> NotFound(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.NotFound, target, errorMessage, errors);
        }

        public static new Result<TValue> InternalServerError<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.InternalServerError, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static new Result<TValue> InternalServerError(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.InternalServerError, target, errorMessage, errors);
        }

        public static new Result<TValue> BadGateway<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.BadGateway, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static new Result<TValue> BadGateway(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.BadGateway, target, errorMessage, errors);
        }

        public static new Result<TValue> ServiceUnavailable<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.ServiceUnavailable, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static new Result<TValue> ServiceUnavailable(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultCode.ServiceUnavailable, target, errorMessage, errors);
        }

        private static Result<TValue> GetResult(ResultCode resultCode, string? target, string? errorMessage, List<Error>? errors)
        {
            return new Result<TValue>() { Status = resultCode, Target = target, ErrorMessage = errorMessage, Errors = errors };
        }

        public Result<TOtherValue> Map<TOtherValue>(Func<TValue, TOtherValue> mapper)
        {
            return new Result<TOtherValue>()
            {
                Status = this.Status,
                Target = this.Target,
                ErrorMessage = this.ErrorMessage,
                Errors = this.Errors,
                Value = this.Value is not null ? mapper(this.Value) : default
            };
        }
    }
}
