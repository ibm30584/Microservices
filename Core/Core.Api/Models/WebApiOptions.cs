using Core.Application.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Core.Api.Models
{
    public class WebApiOptions
    {
        public required string Name { get; set; }
        public bool EnableControllers { get; set; } = true;
        public bool EnableRequestLogging { get; set; } = true;
        public bool EnableExceptionHandlingMiddleware { get; set; } = true;
        public required IHostBuilder Host { get; set; }
        public required IConfiguration Configuration { get; set; }
        public SwaggerOptions? SwaggerOptions { get; set; }

        internal static void Validate(WebApiOptions webApiConfig)
        {
            StartupException.ThrowIfNull(webApiConfig);
            StartupException.ThrowIfNull(webApiConfig.Host);
            StartupException.ThrowIfNull(webApiConfig.Configuration);
        }
    }
}
