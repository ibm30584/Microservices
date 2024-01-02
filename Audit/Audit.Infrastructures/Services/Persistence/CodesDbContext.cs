using Audit.Application.Services.Persistence;
using Audit.Domain.Entities;
using Core.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Audit.Infrastructures.Services.Persistence
{
    public class AuditDbContext : DbContextBase, IAuditDbContext
    {
        public AuditDbContext(DbContextOptions options, ILoggerFactory loggerFactory, IHostEnvironment hostEnvironment, IServiceProvider serviceProvider)
            : base(options, loggerFactory, hostEnvironment, serviceProvider)
        {
        }

        public DbSet<AuditLog> AuditLogs { get; set; }
    }
}