using Core.Application.Enums;
using Core.Application.Exceptions;
using Core.Application.Models;
using Core.Application.Models.CQRS;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace Core.Api.Middlewares
{
    public class HttpHeaderBinderPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequestExtended
        where TResponse : ResultBase
    {
        private readonly HttpContext _httpContext;

        public HttpHeaderBinderPipeline(
            IHttpContextAccessor httpContextAccessor)
        {
            StartupException.ThrowIfNull(httpContextAccessor);
            StartupException.ThrowIfNull(httpContextAccessor.HttpContext);

            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var language = GetLanguage();
            request.Header = GetRequestHeader(language);
            ApplyCulture(language);

            var result = await next();

            BusinessException.ThrowIfNull(result, "Result must not be null");
            BusinessException.ThrowIfNull(result.Header, "Result's header must not be null");
            if (result.Header.StatusCode != AppStatusCode.Ok)
            {
                _httpContext.Response.StatusCode = (int)result.Header.StatusCode;
            }
            _httpContext.Response.Headers["correlationId"] = _httpContext.TraceIdentifier;

            return result;
        }

        private RequestHeader GetRequestHeader(string language)
        {
            return new RequestHeader
            {
                CorrelationId = GetCorrelationId(_httpContext),
                User = _httpContext.User,
                Language = language == AppConstants.ArabicLanguage ? AppLanguage.Arabic : AppLanguage.English,
            };
        }

        private string GetCorrelationId(HttpContext? httpContext)
        {
            return httpContext?.TraceIdentifier ?? Guid.NewGuid().ToString();
        }

        private string GetLanguage()
        {
            var request = _httpContext.Request;
            var culture = request.Headers["Accept-Language"].FirstOrDefault()
                        ?? request.Query["language"].ToString()
                        ?? AppConstants.DefaultLanguage;

            return culture.StartsWith("en", StringComparison.InvariantCultureIgnoreCase)
                 ? AppConstants.EnglishLanguage
                 : AppConstants.ArabicLanguage;
        }

        private static void ApplyCulture(string culture)
        {
            if (string.IsNullOrWhiteSpace(culture))
            {
                return;
            }

            if (Thread.CurrentThread.CurrentUICulture.Name.ToLower() == culture.ToLower())
            {
                return;
            }

            var currentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = currentCulture;
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }
    }
}
