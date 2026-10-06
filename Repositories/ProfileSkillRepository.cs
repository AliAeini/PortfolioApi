using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Interfaces;
using PortfolioApi.Models;

namespace PortfolioApi.Repositories;

public class ProfileSkillRepository : Repository<ProfileSkill>, IProfileSkillRepository
{
    public ProfileSkillRepository(AppDbContext db) : base(db) { }

    public async Task<IEnumerable<ProfileSkill>> GetByProfileAsync(
        Guid profileId, CancellationToken cancellationToken = default)
        => await _dbSet
            .Include(ps => ps.Skill)
                .ThenInclude(s => s.SkillCategory)
            .Where(ps => ps.ProfileId == profileId)
            .OrderBy(ps => ps.DisplayOrder)
            .ToListAsync(cancellationToken);

    public async Task<ProfileSkill?> GetByProfileAndSkillAsync(
        Guid profileId, Guid skillId, CancellationToken cancellationToken = default)
        => await _dbSet
            .Include(ps => ps.Skill)
            .FirstOrDefaultAsync(ps => ps.ProfileId == profileId && ps.SkillId == skillId, cancellationToken);

    public async Task<ProfileSkill?> GetByIdWithSkillAsync(
     Guid id, CancellationToken cancellationToken = default)
     => await _dbSet
         .Include(ps => ps.Skill)
             .ThenInclude(s => s.SkillCategory)
         .AsNoTracking() 
         .FirstOrDefaultAsync(ps => ps.Id == id, cancellationToken);

    public async Task<bool> ExistsAsync(
        Guid profileId, Guid skillId, CancellationToken cancellationToken = default)
        => await _dbSet.AnyAsync(ps => ps.ProfileId == profileId && ps.SkillId == skillId, cancellationToken);
}