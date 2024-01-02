using Core.Application.Models;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;

namespace Core.Infrastructures.Models
{
    public class DbContextBase : DbContext
    {
        protected readonly ILoggerFactory _loggerFactory;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IServiceProvider _serviceProvider;

        public DbContextBase(
            DbContextOptions options,
            ILoggerFactory loggerFactory,
            IHostEnvironment hostEnvironment,
            IServiceProvider serviceProvider) : base(options)
        {
            _loggerFactory = loggerFactory;
            _hostEnvironment = hostEnvironment;
            _serviceProvider = serviceProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            if (_hostEnvironment.IsDevelopment() && Debugger.IsAttached)
            {
                optionsBuilder.EnableSensitiveDataLogging();
            }

            base.OnConfiguring(optionsBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var currentUserName = GetCurrentUserName();
            var systemDate = AppUtilities.GetSystemDate();
            var changes = GetAllChanges();

            TrackCreatedChanges(changes, currentUserName, systemDate);
            TrackUpdatedChanges(changes, currentUserName, systemDate);

            return base.SaveChangesAsync(cancellationToken);

            string GetCurrentUserName()
            {
                var claimsPrincipal = _serviceProvider.GetService<ClaimsPrincipal>();
                var currentUserName = claimsPrincipal?.Identity?.Name ?? "Anonymous";
                return currentUserName;
            }

            List<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry> GetAllChanges()
            {
                return ChangeTracker.Entries().ToList();
            }

            static void TrackCreatedChanges(
                List<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry> changes,
                string currentUserName,
                DateTime systemDate)
            {
                changes
                    .Where(x => x.State == EntityState.Added)
                    .Select(x => x.Entity as ITrackCreatedEntity)
                    .Where(x => x is not null)
                    .ToList()
                    .ForEach(x =>
                    {
                        x!.CreatedByUserId = currentUserName;
                        x.CreatedDate = systemDate;
                    });
            }

            static void TrackUpdatedChanges(
                List<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry> changes,
                string currentUserName,
                DateTime systemDate)
            {
                changes
                    .Where(x => x.State == EntityState.Modified)
                    .Select(x => x.Entity as ITrackUpdatedEntity)
                    .Where(x => x is not null)
                    .ToList()
                    .ForEach(x =>
                    {
                        x!.UpdatedUserId = currentUserName;
                        x.UpdatedDate = systemDate;
                    });
            }
        }
    }
}
