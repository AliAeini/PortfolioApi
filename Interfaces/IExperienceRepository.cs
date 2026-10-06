using PortfolioApi.Models;
using PortfolioApi.Repositories;

namespace PortfolioApi.Interfaces;

public interface IExperienceRepository : IRepository<Experience>
{
    Task<IEnumerable<Experience>> GetByProfileAsync(Guid profileId, CancellationToken cancellationToken = default);
}