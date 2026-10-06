using PortfolioApi.Models;
using PortfolioApi.Repositories;

namespace PortfolioApi.Interfaces;

public interface IEducationRepository : IRepository<Education>
{
    Task<IEnumerable<Education>> GetByProfileAsync(Guid profileId, CancellationToken cancellationToken = default);
}