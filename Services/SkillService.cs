using PortfolioApi.DTOs;
using PortfolioApi.Interfaces;
using PortfolioApi.Models;

namespace PortfolioApi.Services;

public class SkillService : ISkillService
{
    private readonly ISkillRepository _repository;

    public SkillService(ISkillRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SkillDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var skills = await _repository.GetAllWithCategoryAsync(cancellationToken);
        return skills.Select(MapToDto);
    }

    public async Task<SkillDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var skill = await _repository.GetWithCategoryAsync(id, cancellationToken);
        if (skill is null)
            throw new KeyNotFoundException($"Skill with id '{id}' not found");

        return MapToDto(skill);
    }

    public async Task<IEnumerable<GroupedSkillsDto>> GetGroupedAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _repository.GetAllCategoriesAsync(cancellationToken);

        return categories.Select(c => new GroupedSkillsDto(
            c.Id,
            c.Name,
            c.Description,
            c.DisplayOrder,
            c.Skills
                .OrderBy(s => s.DisplayOrder)
                .Select(s => new SkillItemDto(s.Id, s.Name, s.Level, s.DisplayOrder, s.IconUrl))
                .ToList()
        ));
    }

    public async Task<IEnumerable<SkillCategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _repository.GetAllCategoriesAsync(cancellationToken);

        return categories.Select(c => new SkillCategoryDto(
            c.Id,
            c.Name,
            c.Description,
            c.DisplayOrder,
            c.Skills.Count
        ));
    }

    private static SkillDto MapToDto(Skill skill)
        => new(
            skill.Id,
            skill.Name,
            skill.Level,
            skill.DisplayOrder,
            skill.IconUrl,
            skill.SkillCategory is null ? null : new SkillCategoryDto(
                skill.SkillCategory.Id,
                skill.SkillCategory.Name,
                skill.SkillCategory.Description,
                skill.SkillCategory.DisplayOrder,
                skill.SkillCategory.Skills.Count
            )
        );
}