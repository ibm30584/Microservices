using Codes.Domain.Entities;
using Core.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Codes.Application.Services.Persistence
{
    public interface ICodesDbContext : IDbContextBase
    {
        DbSet<Code> Codes { get; set; }
        DbSet<CodeType> CodeTypes { get; set; }
    }
}
