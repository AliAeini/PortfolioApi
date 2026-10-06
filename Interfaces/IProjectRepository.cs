using PortfolioApi.Models;
using PortfolioApi.Repositories;

namespace PortfolioApi.Interfaces;

public interface IProjectRepository : IRepository<Project>
{
    Task<IEnumerable<Project>> GetByProfileAsync(Guid profileId, CancellationToken cancellationToken = default);
    Task<Project?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
}