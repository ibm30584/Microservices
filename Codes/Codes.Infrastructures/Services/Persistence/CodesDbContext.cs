using Codes.Application.Services.Persistence;
using Codes.Domain.Entities;
using Core.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Codes.Infrastructures.Services.Persistence
{
    public class CodesDbContext : DbContextBase, ICodesDbContext
    {
        public CodesDbContext(DbContextOptions options, ILoggerFactory loggerFactory, IHostEnvironment hostEnvironment, IServiceProvider serviceProvider) 
            : base(options, loggerFactory, hostEnvironment, serviceProvider)
        {
        }

        public DbSet<Code> Codes { get; set; }
        public DbSet<CodeType> CodeTypes { get; set; }
    }
}