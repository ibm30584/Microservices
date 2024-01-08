using Codes.Application.Models;
using Codes.Application.Services.Audit;
using Codes.Application.Services.Persistence;
using Codes.Infrastructures.Services;
using Codes.Infrastructures.Services.Persistence;
using Core.Infrastructures.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Codes.Infrastructures.DependencyInjection
{
    public static class IoC
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            CodesConfiguration codesConfiguration,
            Assembly consumersAssembly)
        {
            services
                .AddPersistence(codesConfiguration.ConnectionString)
                .AddAudit();
            services.AddServiceBus(
                codesConfiguration.Messaging,
                consumersAssembly);

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ICodesDbContext, CodesDbContext>(x => x.UseSqlServer(connectionString));
            return services;
        }

        private static IServiceCollection AddAudit(this IServiceCollection services)
        {
            services.AddSingleton<IAuditService, AuditService>();
            return services;
        }

    }
}
