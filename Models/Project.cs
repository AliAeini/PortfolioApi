using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class Project : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string? ShortDescription { get; private set; }
    public string? ImageUrl { get; private set; }
    public string? GithubUrl { get; private set; }
    public string? LiveUrl { get; private set; }
    public string? TechStack { get; private set; }
    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public bool IsFeatured { get; private set; }
    public int DisplayOrder { get; private set; }

    public Guid? ProjectCategoryId { get; private set; }
    public ProjectCategory? ProjectCategory { get; private set; }

    private Project() { }

    public static Project Create(
        string title,
        string description,
        Guid? projectCategoryId = null,
        string? shortDescription = null,
        string? imageUrl = null,
        string? githubUrl = null,
        string? liveUrl = null,
        string? techStack = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        bool isFeatured = false,
        int displayOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        return new Project
        {
            Title = title.Trim(),
            Description = description.Trim(),
            ProjectCategoryId = projectCategoryId,
            ShortDescription = shortDescription?.Trim(),
            ImageUrl = imageUrl?.Trim(),
            GithubUrl = githubUrl?.Trim(),
            LiveUrl = liveUrl?.Trim(),
            TechStack = techStack?.Trim(),
            StartDate = startDate,
            EndDate = endDate,
            IsFeatured = isFeatured,
            DisplayOrder = displayOrder
        };
    }

    public void Update(
        string title, string description,
        Guid? projectCategoryId, string? shortDescription,
        string? imageUrl, string? githubUrl, string? liveUrl,
        string? techStack, DateTime? startDate, DateTime? endDate,
        bool isFeatured, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        Title = title.Trim();
        Description = description.Trim();
        ProjectCategoryId = projectCategoryId;
        ShortDescription = shortDescription?.Trim();
        ImageUrl = imageUrl?.Trim();
        GithubUrl = githubUrl?.Trim();
        LiveUrl = liveUrl?.Trim();
        TechStack = techStack?.Trim();
        StartDate = startDate;
        EndDate = endDate;
        IsFeatured = isFeatured;
        DisplayOrder = displayOrder;
        UpdateTimestamp();
    }
}