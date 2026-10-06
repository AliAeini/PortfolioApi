using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class Project : BaseEntity
{
    public Guid ProfileId { get; private set; }
    public Profile Profile { get; private set; } = null!;
    public Guid? ProjectCategoryId { get; private set; }
    public ProjectCategory? ProjectCategory { get; private set; }
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string? ShortDescription { get; private set; }
    public string? GithubUrl { get; private set; }
    public string? LiveUrl { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public bool IsFeatured { get; private set; }
    public int DisplayOrder { get; private set; }

    public List<ProjectSkillRef> ProjectSkills { get; private set; } = new();
    public List<ProjectImage> Images { get; private set; } = new();

    private Project() { }

    public static Project Create(
        Guid profileId,
        string title,
        string description,
        Guid? projectCategoryId = null,
        string? shortDescription = null,
        string? githubUrl = null,
        string? liveUrl = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        bool isFeatured = false,
        int displayOrder = 0)
    {
        if (profileId == Guid.Empty)
            throw new ArgumentException("Profile ID is required.", nameof(profileId));

        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        if (title.Length > 200)
            throw new ArgumentException("Title must not exceed 200 characters.", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        if (description.Length > 5000)
            throw new ArgumentException("Description must not exceed 5000 characters.", nameof(description));

        if (endDate.HasValue && startDate.HasValue && endDate.Value < startDate.Value)
            throw new ArgumentException("End date cannot be before start date.", nameof(endDate));

        return new Project
        {
            ProfileId = profileId,
            Title = title.Trim(),
            Description = description.Trim(),
            ProjectCategoryId = projectCategoryId,
            ShortDescription = string.IsNullOrWhiteSpace(shortDescription) ? null : shortDescription.Trim(),
            GithubUrl = string.IsNullOrWhiteSpace(githubUrl) ? null : githubUrl.Trim(),
            LiveUrl = string.IsNullOrWhiteSpace(liveUrl) ? null : liveUrl.Trim(),
            StartDate = startDate,
            EndDate = endDate,
            IsFeatured = isFeatured,
            DisplayOrder = displayOrder,
            Status = EntityStatus.Active
        };
    }

    public void Update(
        string title,
        string description,
        Guid? projectCategoryId,
        string? shortDescription,
        string? githubUrl,
        string? liveUrl,
        DateTime? startDate,
        DateTime? endDate,
        bool isFeatured,
        int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        if (endDate.HasValue && startDate.HasValue && endDate.Value < startDate.Value)
            throw new ArgumentException("End date cannot be before start date.", nameof(endDate));

        Title = title.Trim();
        Description = description.Trim();
        ProjectCategoryId = projectCategoryId;
        ShortDescription = string.IsNullOrWhiteSpace(shortDescription) ? null : shortDescription.Trim();
        GithubUrl = string.IsNullOrWhiteSpace(githubUrl) ? null : githubUrl.Trim();
        LiveUrl = string.IsNullOrWhiteSpace(liveUrl) ? null : liveUrl.Trim();
        StartDate = startDate;
        EndDate = endDate;
        IsFeatured = isFeatured;
        DisplayOrder = displayOrder;
    }

    public void AddProfileSkill(Guid profileSkillId, int displayOrder = 0)
    {
        if (ProjectSkills.Any(ps => ps.ProfileSkillId == profileSkillId)) return;
        ProjectSkills.Add(ProjectSkillRef.Create(Id, profileSkillId, displayOrder));
    }
}