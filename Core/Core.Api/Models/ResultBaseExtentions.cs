using Core.Application.Exceptions;
using Core.Application.Models.CQRS;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Core.Api.Models
{
    public static class ResultBaseExtentions
    {
        public static async Task<bool> EnsureSuccessAsync<TResult>(this TResult result, HttpContext httpContext)
            where TResult : ResultBase
        {
            BusinessException.ThrowIfNull(result, "Result must not be null");

            var header = result.Header;
            BusinessException.ThrowIfNull(header, "Result's header must not be null");
            if (header.ErrorCode == Application.Enums.AppErrorCode.Ok)
            {
                return true;
            }

            // Set the response status code
            httpContext.Response.StatusCode = (int)header.ErrorCode;
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.Headers["correlationId"] = httpContext.TraceIdentifier;

            // Create a custom error response
            var errorResponse = new
            {
                CorrelationId = httpContext.TraceIdentifier,
                header.ErrorCode,
                header.ErrorMessage,
                header.Errors
            };

            // Serialize the error response and write it to the response
            var jsonResponse = JsonConvert.SerializeObject(errorResponse);
            await httpContext.Response.WriteAsync(jsonResponse);
            return false;
        }
    }
}
