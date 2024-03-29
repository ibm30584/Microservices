﻿using Audit.Application.Models;
using Audit.Application.Services.Persistence;
using Audit.Infrastructures.Services.Persistence;
using Core.Infrastructures.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Audit.Infrastructures.DependencyInjection
{
    public static class IoC
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            AuditConfiguration auditConfiguration,
            Assembly consumersAssembly)
        {
            services
                .AddPersistence(auditConfiguration.ConnectionString)
                //.AddCustomService()
                ;
            
            services.AddServiceBus(
                auditConfiguration.Messaging,
                consumersAssembly);

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<IAuditDbContext, AuditDbContext>(x => x.UseSqlServer(connectionString));
            return services;
        }

        //private static IServiceCollection AddCustomService(this IServiceCollection services)
        //{
        //    services.AddSingleton<ICustomService, CustomService>();
        //    return services;
        //}
    }
}
