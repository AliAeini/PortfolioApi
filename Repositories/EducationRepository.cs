using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Interfaces;
using PortfolioApi.Models;

namespace PortfolioApi.Repositories;

public class EducationRepository : Repository<Education>, IEducationRepository
{
    public EducationRepository(AppDbContext db) : base(db) { }

    public async Task<IEnumerable<Education>> GetByProfileAsync(
        Guid profileId, CancellationToken cancellationToken = default)
        => await _dbSet
            .Where(e => e.ProfileId == profileId)
            .OrderByDescending(e => e.StartDate)
            .ThenBy(e => e.DisplayOrder)
            .ToListAsync(cancellationToken);
}