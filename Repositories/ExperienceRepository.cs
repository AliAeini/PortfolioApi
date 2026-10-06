using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Interfaces;
using PortfolioApi.Models;

namespace PortfolioApi.Repositories;

public class ExperienceRepository : Repository<Experience>, IExperienceRepository
{
    public ExperienceRepository(AppDbContext db) : base(db) { }

    public async Task<IEnumerable<Experience>> GetByProfileAsync(
        Guid profileId, CancellationToken cancellationToken = default)
        => await _dbSet
            .Where(e => e.ProfileId == profileId)
            .OrderByDescending(e => e.StartDate)
            .ThenBy(e => e.DisplayOrder)
            .ToListAsync(cancellationToken);
}