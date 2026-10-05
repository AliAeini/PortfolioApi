using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class Skill : BaseEntity
{
    public string Name { get; private set; } = null!;
    public int Level { get; private set; }
    public int DisplayOrder { get; private set; }
    public string? IconUrl { get; private set; }

    public Guid SkillCategoryId { get; private set; }
    public SkillCategory SkillCategory { get; private set; } = null!;
    public ICollection<ProfileSkill> ProfileSkills { get; private set; } = new List<ProfileSkill>();
    private Skill() { }

    public static Skill Create(
        string name,
        int level,
        Guid skillCategoryId,
        int displayOrder = 0,
        string? iconUrl = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Skill name is required.", nameof(name));

        if (level < 0 || level > 100)
            throw new ArgumentException("Level must be between 0 and 100.", nameof(level));

        return new Skill
        {
            Name = name.Trim(),
            Level = level,
            SkillCategoryId = skillCategoryId,
            DisplayOrder = displayOrder,
            IconUrl = iconUrl?.Trim()
        };
    }

    public void Update(string name, int level, Guid skillCategoryId, int displayOrder, string? iconUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Skill name is required.", nameof(name));

        if (level < 0 || level > 100)
            throw new ArgumentException("Level must be between 0 and 100.", nameof(level));

        Name = name.Trim();
        Level = level;
        SkillCategoryId = skillCategoryId;
        DisplayOrder = displayOrder;
        IconUrl = iconUrl?.Trim();
        UpdateTimestamp();
    }
}