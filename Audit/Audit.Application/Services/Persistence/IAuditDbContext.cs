using Core.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Audit.Application.Services.Persistence
{
    public interface IAuditDbContext : IDbContextBase
    {
        DbSet<Domain.Entities.AuditLog> AuditLogs { get; set; }
        DbSet<Domain.Entities.Event> Events { get; set; }
        DbSet<Domain.Entities.Service> Services { get; set; }
    }
}
