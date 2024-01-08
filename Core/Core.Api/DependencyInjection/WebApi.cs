using Core.Api.Middlewares;
using Core.Api.Models;
using Core.Application.Models;
using Core.Infrastructures.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;

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
                services.AddControllers()
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    });
            }

            var swaggerConfig = webApiOptions.SwaggerOptions;
            if (swaggerConfig is not null && swaggerConfig.Enabled)
            {
                services.AddSwagger();
            }

            services.AddHttpContextAccessor();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(HttpHeaderBinderPipeline<,>));

            AppConstants.CurrentLanguage = webApiOptions.DefaultLanguage;
            return logger;
        }

        public static IApplicationBuilder UseWebApi(
            this WebApplication app,
            WebApiOptions webApiOptions)
        {
            if (webApiOptions.EnableRequestLogging)
            {
                app.UseRequestLogging();
            }

            if (webApiOptions.EnableExceptionHandlingMiddleware)
            {
                app.UseExceptionMiddleware();
            }

            if (webApiOptions.EnableControllers)
            {
                app.MapControllers();
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
