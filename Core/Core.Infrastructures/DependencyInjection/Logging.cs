using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using NativeLogger = Microsoft.Extensions.Logging.ILogger;

namespace Core.Infrastructures.DependencyInjection
{
    public static class Logging
    {
        public static NativeLogger AddLogging(this IHostBuilder host, string applicationName, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            host.UseSerilog();

            var loggerFactory = LoggerFactory.Create(
                builder =>
            {
                builder.AddSerilog(Log.Logger);
            });

            // Get an instance of ILogger<T>
            var logger = loggerFactory.CreateLogger(applicationName);

            return logger;
        }
        public static void UseRequestLogging(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();
        }
        public static void CloseAndFlush(this NativeLogger _)
        {
            Log.CloseAndFlush();
        }
    }
}
