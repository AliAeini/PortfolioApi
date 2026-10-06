using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class Experience : BaseEntity
{
    public Guid ProfileId { get; private set; }
    public Profile Profile { get; private set; } = null!;

    public string Company { get; private set; } = null!;
    public string Position { get; private set; } = null!;
    public EmploymentType EmploymentType { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string? Description { get; private set; }
    public string? Location { get; private set; }
    public string? CompanyUrl { get; private set; }
    public int DisplayOrder { get; private set; }

    private Experience() { }

    public static Experience Create(
        Guid profileId,
        string company,
        string position,
        EmploymentType employmentType,
        DateTime startDate,
        DateTime? endDate = null,
        string? description = null,
        string? location = null,
        string? companyUrl = null,
        int displayOrder = 0)
    {
        if (profileId == Guid.Empty)
            throw new ArgumentException("Profile ID is required.", nameof(profileId));

        if (string.IsNullOrWhiteSpace(company))
            throw new ArgumentException("Company is required.", nameof(company));

        if (company.Length > 200)
            throw new ArgumentException("Company must not exceed 200 characters.", nameof(company));

        if (string.IsNullOrWhiteSpace(position))
            throw new ArgumentException("Position is required.", nameof(position));

        if (position.Length > 200)
            throw new ArgumentException("Position must not exceed 200 characters.", nameof(position));

        if (!Enum.IsDefined(typeof(EmploymentType), employmentType))
            throw new ArgumentException("Invalid employment type.", nameof(employmentType));

        if (startDate == default)
            throw new ArgumentException("Start date is required.", nameof(startDate));

        if (endDate.HasValue && endDate.Value < startDate)
            throw new ArgumentException("End date cannot be before start date.", nameof(endDate));

        return new Experience
        {
            ProfileId = profileId,
            Company = company.Trim(),
            Position = position.Trim(),
            EmploymentType = employmentType,
            StartDate = startDate,
            EndDate = endDate,
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim(),
            CompanyUrl = string.IsNullOrWhiteSpace(companyUrl) ? null : companyUrl.Trim(),
            DisplayOrder = displayOrder,
            Status = EntityStatus.Active
        };
    }

    public void Update(
        string company,
        string position,
        EmploymentType employmentType,
        DateTime startDate,
        DateTime? endDate,
        string? description,
        string? location,
        string? companyUrl,
        int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(company))
            throw new ArgumentException("Company is required.", nameof(company));

        if (string.IsNullOrWhiteSpace(position))
            throw new ArgumentException("Position is required.", nameof(position));

        if (!Enum.IsDefined(typeof(EmploymentType), employmentType))
            throw new ArgumentException("Invalid employment type.", nameof(employmentType));

        if (endDate.HasValue && endDate.Value < startDate)
            throw new ArgumentException("End date cannot be before start date.", nameof(endDate));

        Company = company.Trim();
        Position = position.Trim();
        EmploymentType = employmentType;
        StartDate = startDate;
        EndDate = endDate;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim();
        CompanyUrl = string.IsNullOrWhiteSpace(companyUrl) ? null : companyUrl.Trim();
        DisplayOrder = displayOrder;
    }
}