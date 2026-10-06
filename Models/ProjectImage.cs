using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class ProjectImage : BaseEntity
{
    public Guid ProjectId { get; private set; }
    public Project Project { get; private set; } = null!;

    public string ImageUrl { get; private set; } = null!;
    public string? Caption { get; private set; }
    public bool IsCover { get; private set; }
    public int DisplayOrder { get; private set; }

    private ProjectImage() { }

    public static ProjectImage Create(
        Guid projectId,
        string imageUrl,
        string? caption = null,
        bool isCover = false,
        int displayOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("Image URL is required.", nameof(imageUrl));

        return new ProjectImage
        {
            ProjectId = projectId,
            ImageUrl = imageUrl.Trim(),
            Caption = string.IsNullOrWhiteSpace(caption) ? null : caption.Trim(),
            IsCover = isCover,
            DisplayOrder = displayOrder,
            Status = EntityStatus.Active
        };
    }
}