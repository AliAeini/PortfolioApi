using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Interfaces;
using PortfolioApi.Models;

namespace PortfolioApi.Repositories;

public class ProjectRepository : Repository<Project>, IProjectRepository
{
    public ProjectRepository(AppDbContext db) : base(db) { }

    public async Task<IEnumerable<Project>> GetByProfileAsync(
        Guid profileId, CancellationToken cancellationToken = default)
        => await _dbSet
            .Include(p => p.ProjectCategory)
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.ProfileSkill)
                    .ThenInclude(psk => psk.Skill)
                        .ThenInclude(s => s.SkillCategory)
            .Include(p => p.Images)
            .Where(p => p.ProfileId == profileId)
            .OrderByDescending(p => p.IsFeatured)
            .ThenBy(p => p.DisplayOrder)
            .ToListAsync(cancellationToken);

    public async Task<Project?> GetByIdWithDetailsAsync(
        Guid id, CancellationToken cancellationToken = default)
        => await _dbSet
            .Include(p => p.ProjectCategory)
            .Include(p => p.ProjectSkills)
                .ThenInclude(ps => ps.ProfileSkill)
                    .ThenInclude(psk => psk.Skill)
                        .ThenInclude(s => s.SkillCategory)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
}