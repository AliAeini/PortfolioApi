using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Interfaces;
using PortfolioApi.Models;

namespace PortfolioApi.Repositories;

public class SocialLinkRepository : Repository<SocialLink>, ISocialLinkRepository
{
    public SocialLinkRepository(AppDbContext db) : base(db) { }

    public async Task<IEnumerable<SocialLink>> GetByProfileAsync(
        Guid profileId, CancellationToken cancellationToken = default)
        => await _dbSet
            .Where(s => s.ProfileId == profileId)
            .OrderBy(s => s.DisplayOrder)
            .ToListAsync(cancellationToken);
}