using Core.Application.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Audit.Application.DependencyInjection
{
    public static class IoC
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddCQRS(typeof(IoC).Assembly);
            return services;
        }
    }
}
