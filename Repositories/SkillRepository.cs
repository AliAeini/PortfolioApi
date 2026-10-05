using Microsoft.EntityFrameworkCore;
using PortfolioApi.Data;
using PortfolioApi.Interfaces;
using PortfolioApi.Models;

namespace PortfolioApi.Repositories;

public class SkillRepository : Repository<Skill>, ISkillRepository
{
    public SkillRepository(AppDbContext db) : base(db) { }

    public async Task<IEnumerable<Skill>> GetAllWithCategoryAsync(CancellationToken cancellationToken = default)
        => await _dbSet
            .Include(s => s.SkillCategory)
            .OrderBy(s => s.SkillCategory.DisplayOrder)
            .ThenBy(s => s.DisplayOrder)
            .ToListAsync(cancellationToken);

    public async Task<Skill?> GetWithCategoryAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbSet
            .Include(s => s.SkillCategory)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public async Task<IEnumerable<Skill>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default)
        => await _dbSet
            .Where(s => s.SkillCategoryId == categoryId)
            .OrderBy(s => s.DisplayOrder)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<SkillCategory>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
        => await _db.Set<SkillCategory>()
            .Include(c => c.Skills)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync(cancellationToken);

    public async Task<SkillCategory?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _db.Set<SkillCategory>()
            .Include(c => c.Skills)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
}