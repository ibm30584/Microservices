namespace Core.Application.Models
{
    public interface IDbContextBase
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
