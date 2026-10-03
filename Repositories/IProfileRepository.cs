using PortfolioApi.Models;

namespace PortfolioApi.Repositories;

public interface IProfileRepository : IRepository<Profile>
{
    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null, CancellationToken cancellationToken = default);
    Task<Profile?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}