using Core.Application.Enums;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Core.Application.Models.CQRS
{
    public class Result
    {
        internal Result()
        {
        }
        [JsonPropertyOrder(-4)]
        public ResultStatus Status { get; set; } = ResultStatus.Ok;
        [JsonPropertyOrder(-3)]
        public string? ErrorMessage { get; set; }
        [JsonPropertyOrder(-2)]
        public string? Target { get; set; }
        [JsonPropertyOrder(-1)]
        public List<Error>? Errors { get; set; }


        public static readonly Result OK = new() { Status = ResultStatus.Ok };
        public static Result<TValue> Ok<TValue>(TValue value)
        {
            return new Result<TValue>() { Status = ResultStatus.Ok, Value = value };
        }

        public static Result BadRequest<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.BadRequest, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static Result BadRequest(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.BadRequest, target, errorMessage, errors);
        }


        public static Result Unauthorized<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.Unauthorized, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static Result Unauthorized(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.Unauthorized, target, errorMessage, errors);
        }


        public static Result Forbidden<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.Forbidden, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static Result Forbidden(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.Forbidden, target, errorMessage, errors);
        }

        public static Result NotFound<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.NotFound, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static Result NotFound(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.NotFound, target, errorMessage, errors);
        }

        public static Result InternalServerError<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.InternalServerError, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static Result InternalServerError(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.InternalServerError, target, errorMessage, errors);
        }

        public static Result BadGateway<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.BadGateway, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static Result BadGateway(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.BadGateway, target, errorMessage, errors);
        }

        public static Result ServiceUnavailable<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.ServiceUnavailable, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static Result ServiceUnavailable(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.ServiceUnavailable, target, errorMessage, errors);
        }


        private static Result GetResult(ResultStatus resultCode, string? target, string? errorMessage, List<Error>? errors)
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
            return new Result<TValue>() { Status = ResultStatus.Ok, Value = value };
        }

        public static new Result<TValue> BadRequest<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.BadRequest, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static new Result<TValue> BadRequest(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.BadRequest, target, errorMessage, errors);
        }

        public static new Result<TValue> Unauthorized<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.Unauthorized, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static new Result<TValue> Unauthorized(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.Unauthorized, target, errorMessage, errors);
        }

        public static new Result<TValue> Forbidden<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.Forbidden, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static new Result<TValue> Forbidden(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.Forbidden, target, errorMessage, errors);
        }

        public static new Result<TValue> NotFound<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.NotFound, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static new Result<TValue> NotFound(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.NotFound, target, errorMessage, errors);
        }

        public static new Result<TValue> InternalServerError<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.InternalServerError, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static new Result<TValue> InternalServerError(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.InternalServerError, target, errorMessage, errors);
        }

        public static new Result<TValue> BadGateway<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.BadGateway, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static new Result<TValue> BadGateway(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.BadGateway, target, errorMessage, errors);
        }

        public static new Result<TValue> ServiceUnavailable<TRequest>(Expression<Func<TRequest, object>> target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.ServiceUnavailable, AppUtilities.GetFieldName(target), errorMessage, errors);
        }
        public static new Result<TValue> ServiceUnavailable(string target, string? errorMessage, List<Error>? errors = null)
        {
            return GetResult(ResultStatus.ServiceUnavailable, target, errorMessage, errors);
        }

        private static Result<TValue> GetResult(ResultStatus resultCode, string? target, string? errorMessage, List<Error>? errors)
        {
            return new Result<TValue>() { Status = resultCode, Target = target, ErrorMessage = errorMessage, Errors = errors };
        }

        public Result<TOtherValue> Map<TOtherValue>(Func<TValue, TOtherValue> mapper)
        {
            return new Result<TOtherValue>()
            {
                Status = Status,
                Target = Target,
                ErrorMessage = ErrorMessage,
                Errors = Errors,
                Value = Value is not null ? mapper(Value) : default
            };
        }
    }
}
