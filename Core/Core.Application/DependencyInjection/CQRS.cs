using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Application.DependencyInjection
{
    public static class CQRS
    {
        public static IServiceCollection AddCQRS(this IServiceCollection services, Assembly assembly)
        {
            services.AddMediatR(x => x.RegisterServicesFromAssembly(assembly));
            return services;
        }
    }
}
