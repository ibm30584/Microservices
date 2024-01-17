using Core.Application.Enums;
using Core.Application.Exceptions;
using Core.Application.Models;
using Core.Application.Models.CQRS;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace Core.Api.Middlewares
{
    public class CQRSPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : notnull
    {
        private readonly HttpContext _httpContext;

        public CQRSPipeline(
            IHttpContextAccessor httpContextAccessor)
        {
            StartupException.ThrowIfNull(httpContextAccessor);
            StartupException.ThrowIfNull(httpContextAccessor.HttpContext);

            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var language = GetLanguage();
            ApplyCulture(language);

            var result = await next();


            if (result is Result resultInstance && resultInstance.Status != ResultStatus.Ok)
            {
                _httpContext.Response.StatusCode = (int)resultInstance.Status;
            }
            _httpContext.Response.Headers["correlationId"] = _httpContext.TraceIdentifier;

            return result;
        }

        private string GetLanguage()
        {
            var request = _httpContext.Request;
            var culture = request.Headers.AcceptLanguage.FirstOrDefault()
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

            var currentUICulture = Thread.CurrentThread.CurrentUICulture.Name;
            if (currentUICulture.Equals(culture, StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }

            var currentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = currentCulture;
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }
    }
}
