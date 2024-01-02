using Codes.Application.DependencyInjection;
using Codes.Application.Models;
using Codes.Infrastructures.DependencyInjection;
using Core.Api.DependencyInjection;
using Core.Api.Models;
using Core.Infrastructures.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
ILogger logger = null!;


try
{
    var webApiOptions = new WebApiOptions
    {
        Name = "Codes Service",
        EnableControllers = true,
        EnableRequestLogging = true,
        EnableExceptionHandlingMiddleware = true,
        Host = builder.Host,
        Configuration = builder.Configuration,
        SwaggerOptions = new SwaggerOptions
        {
            Enabled = true,
            Route = "codes-swagger"
        }
    };
    var codesConfiguration = CodesConfiguration.CreateFrom(builder.Configuration);

    logger = builder.Services.AddWebApi(webApiOptions);
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(codesConfiguration);

    var app = builder.Build();
    app.UseWebApi(webApiOptions);

    app.Run();
}
catch (HostAbortedException) { }
catch (Exception exp)
{
    if (logger == null)
    {
        Console.WriteLine($"Application failed with error: {exp.Message}");
    }
    else
    {
        logger.LogError(exp, "Application failed with error: {error}", exp.Message);
    }
}
finally
{
    logger?.CloseAndFlush();
}