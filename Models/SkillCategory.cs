using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class SkillCategory : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public int DisplayOrder { get; private set; }

    public ICollection<Skill> Skills { get; private set; } = new List<Skill>();

    private SkillCategory() { }

    public static SkillCategory Create(string name, string? description = null, int displayOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required.", nameof(name));

        return new SkillCategory
        {
            Name = name.Trim(),
            Description = description?.Trim(),
            DisplayOrder = displayOrder,
            Status = EntityStatus.Active
        };
    }

    public void Update(string name, string? description, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required.", nameof(name));

        Name = name.Trim();
        Description = description?.Trim();
        DisplayOrder = displayOrder;
        UpdateTimestamp();
    }
}