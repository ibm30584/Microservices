using Core.Api.Filters;
using Core.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Core.Api.DependencyInjection
{
    public static class Swagger
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer"
                });

                // add auth header for [Authorize] endpoints
                options.OperationFilter<AddAuthHeaderOperationFilter>();
            });
            return services;
        }
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, string applicationName, SwaggerOptions swaggerConfig)
        {
            if (swaggerConfig is null || !swaggerConfig.Enabled)
            {
                return app;
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", applicationName);
                options.RoutePrefix = swaggerConfig.Route;
            });
            return app;
        }
    }
}
