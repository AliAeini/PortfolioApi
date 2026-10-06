using PortfolioApi.Models;
using PortfolioApi.Repositories;

namespace PortfolioApi.Interfaces;

public interface ISocialLinkRepository : IRepository<SocialLink>
{
    Task<IEnumerable<SocialLink>> GetByProfileAsync(Guid profileId, CancellationToken cancellationToken = default);
}