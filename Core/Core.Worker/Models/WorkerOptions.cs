using Core.Application.Exceptions;
using Core.Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.Worker.Models
{
    public class WorkerOptions
    {
        public required string Name { get; set; }
        public required HostApplicationBuilder Builder { get; set; }
        public required IConfiguration Configuration { get; set; }
        public string DefaultLanguage { get; set; } = AppConstants.DefaultLanguage;

        internal static void Validate(WorkerOptions processorOptions)
        {
            StartupException.ThrowIfNull(processorOptions);
            StartupException.ThrowIfNull(processorOptions.Builder);
            StartupException.ThrowIfNull(processorOptions.Configuration);
        }
    }
}
