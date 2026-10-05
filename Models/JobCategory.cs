using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class JobCategory : BaseEntity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public int DisplayOrder { get; private set; }

    public ICollection<Profile> Profiles { get; private set; } = new List<Profile>();

    private JobCategory() { }

    public static JobCategory Create(string name, string? description = null, int displayOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Job category name is required.", nameof(name));

        return new JobCategory
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
            throw new ArgumentException("Job category name is required.", nameof(name));

        Name = name.Trim();
        Description = description?.Trim();
        DisplayOrder = displayOrder;
        UpdateTimestamp();
    }
}