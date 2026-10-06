using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class ProfileSkill : BaseEntity
{
    public Guid ProfileId { get; private set; }
    public Profile Profile { get; private set; } = null!;
    public Guid SkillId { get; private set; }
    public Skill Skill { get; private set; } = null!;
    public int Level { get; private set; }
    public int DisplayOrder { get; private set; }

    public ICollection<ProjectSkillRef> ProjectSkillRefs { get; private set; } = new List<ProjectSkillRef>();

    private ProfileSkill() { }

    public static ProfileSkill Create(
        Guid profileId,
        Guid skillId,
        int level = 50,
        int displayOrder = 0)
    {
        if (level < 0 || level > 100)
            throw new ArgumentException("Level must be between 0 and 100.", nameof(level));

        return new ProfileSkill
        {
            ProfileId = profileId,
            SkillId = skillId,
            Level = level,
            DisplayOrder = displayOrder,
            Status = EntityStatus.Active
        };
    }

    public void Update(int level, int displayOrder)
    {
        if (level < 0 || level > 100)
            throw new ArgumentException("Level must be between 0 and 100.", nameof(level));

        Level = level;
        DisplayOrder = displayOrder;
        UpdateTimestamp();
    }
}