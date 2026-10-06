using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class ProjectSkillRef : BaseEntity
{
    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;

    public Guid ProfileSkillId { get; private set; } 
    public ProfileSkill ProfileSkill { get; private set; } = null!;

    public int DisplayOrder { get; private set; }

    private ProjectSkillRef() { }

    public static ProjectSkillRef Create(Guid projectId, Guid profileSkillId, int displayOrder = 0)
    {
        return new ProjectSkillRef
        {
            ProjectId = projectId,
            ProfileSkillId = profileSkillId,
            DisplayOrder = displayOrder,
            Status = EntityStatus.Active
        };
    }

}