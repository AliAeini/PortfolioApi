using PortfolioApi.Models;
using PortfolioApi.Repositories;

namespace PortfolioApi.Interfaces;

public interface ISkillRepository : IRepository<Skill>
{
    Task<IEnumerable<Skill>> GetAllWithCategoryAsync(CancellationToken cancellationToken = default);
    Task<Skill?> GetWithCategoryAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Skill>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SkillCategory>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
    Task<SkillCategory?> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default);
}