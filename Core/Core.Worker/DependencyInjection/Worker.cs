using Core.Application.Models;
using Core.Infrastructures.DependencyInjection;
using Core.Worker.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Processor.DependencyInjection
{
    public static class Worker
    {
        public static ILogger AddWorker(
            this IServiceCollection _,
            WorkerOptions workerOptions)
        {
            WorkerOptions.Validate(workerOptions);

            var logger = workerOptions.Builder.AddLogging(workerOptions.Name, workerOptions.Configuration);

            AppConstants.DefaultLanguage = workerOptions.DefaultLanguage;
            return logger;
        }
    }
}
