using Audit.Application.DependencyInjection;
using Audit.Application.Models;
using Audit.Infrastructures.DependencyInjection;
using Core.Api.DependencyInjection;
using Core.Api.Models;
using Core.Infrastructures.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
ILogger logger = null!;

try
{
    var webApiOptions = new WebApiOptions
    {
        Name = "Audit Service",
        EnableControllers = true,
        EnableRequestLogging = true,
        EnableExceptionHandlingMiddleware = true,
        Host = builder.Host,
        Configuration = builder.Configuration,
        SwaggerOptions = new SwaggerOptions
        {
            Enabled = true,
            Route = "audit-api"
        }
    };
    var auditConfiguration = AuditConfiguration.CreateFrom(builder.Configuration);

    logger = builder.Services.AddWebApi(webApiOptions);
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(auditConfiguration,
        typeof(Program).Assembly);

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