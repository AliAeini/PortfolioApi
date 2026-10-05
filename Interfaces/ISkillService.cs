using PortfolioApi.DTOs;

namespace PortfolioApi.Interfaces;

public interface ISkillService
{
    Task<IEnumerable<SkillDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<SkillDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupedSkillsDto>> GetGroupedAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SkillCategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);
}