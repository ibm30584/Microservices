using Core.Application.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Context;
using System.Net;

namespace Core.Api.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (BusinessException exp)
            {
                await HandleBusinessExceptionAsync(httpContext, exp);
            }
            catch (ProcessingException exp)
            {
                await HandleProcessingExceptionAsync(httpContext, exp);
            }
            catch (Exception exp)
            {
                await HandleExceptionAsync(httpContext, exp);
            }
        }



        private async Task HandleBusinessExceptionAsync(HttpContext context, BusinessException exp)
        {
            _logger.LogError(
                "Business error occurred : {businessError}", 
                exp.Message);

            // Set the response status code
            context.Response.StatusCode = (int)exp.ErrorCode;
            context.Response.ContentType = "application/json";
            context.Response.Headers["correlationId"] = context.TraceIdentifier;

            // Create a custom error response
            var errorResponse = new
            {
                CorrelationId = context.TraceIdentifier,
                context.Response.StatusCode,
                exp.Message
            };

            // Serialize the error response and write it to the response
            var jsonResponse = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
        }
        private async Task HandleProcessingExceptionAsync(HttpContext context, ProcessingException exp)
        {
            _logger.LogError(exp,
                "Processing error occurred : {processingError}",
                exp.Message);

            // Set the response status code
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            context.Response.Headers["correlationId"] = context.TraceIdentifier;

            // Create a custom error response
            var errorResponse = new
            {
                CorrelationId = context.TraceIdentifier,
                context.Response.StatusCode,
                exp.Message
            };

            // Serialize the error response and write it to the response
            var jsonResponse = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exp)
        {

            _logger.LogError(exp,
                "Unexpected error occurred : {unexpectedError}",
                exp.Message);

            // Set the response status code
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            context.Response.Headers["correlationId"] = context.TraceIdentifier;

            // Create a custom error response
            var errorResponse = new
            {
                CorrelationId = context.TraceIdentifier,
                context.Response.StatusCode,
                exp.Message
            };

            // Serialize the error response and write it to the response
            var jsonResponse = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
        }
    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
