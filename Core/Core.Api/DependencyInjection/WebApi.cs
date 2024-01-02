using Core.Api.Middlewares;
using Core.Api.Models;
using Core.Infrastructures.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Api.DependencyInjection
{
    public static class WebApi
    {
        public static ILogger AddWebApi(
            this IServiceCollection services,
            WebApiOptions webApiOptions)
        {
            WebApiOptions.Validate(webApiOptions);

            var logger = webApiOptions.Host.AddLogging(webApiOptions.Name, webApiOptions.Configuration);

            if (webApiOptions.EnableControllers)
            {
                services.AddControllers();
            }

            var swaggerConfig = webApiOptions.SwaggerOptions;
            if (swaggerConfig is not null && swaggerConfig.Enabled)
            {
                services.AddSwagger();
            }

            return logger;
        }

        public static IApplicationBuilder UseWebApi(
            this WebApplication app,
            WebApiOptions webApiOptions)
        {
            if (webApiOptions.EnableControllers)
            {
                app.MapControllers();
            }

            if (webApiOptions.EnableRequestLogging)
            {
                app.UseRequestLogging();
            }

            if (webApiOptions.EnableExceptionHandlingMiddleware)
            {
                app.UseExceptionMiddleware();
            }

            var swaggerConfig = webApiOptions.SwaggerOptions;
            if (swaggerConfig is not null && swaggerConfig.Enabled)
            {
                app.UseSwagger(webApiOptions.Name, swaggerConfig);
            }

            return app;
        }
    }
}
