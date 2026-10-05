using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class Experience : BaseEntity
{
    public string Company { get; private set; } = null!;
    public string Position { get; private set; } = null!;
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string? Description { get; private set; }
    public string? Location { get; private set; }
    public string? CompanyUrl { get; private set; }
    public string? EmploymentType { get; private set; }
    public int DisplayOrder { get; private set; }

    private Experience() { }

    public static Experience Create(
        string company,
        string position,
        DateTime startDate,
        DateTime? endDate = null,
        string? description = null,
        string? location = null,
        string? companyUrl = null,
        string? employmentType = null,
        int displayOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(company))
            throw new ArgumentException("Company is required.", nameof(company));

        if (string.IsNullOrWhiteSpace(position))
            throw new ArgumentException("Position is required.", nameof(position));

        if (endDate.HasValue && endDate < startDate)
            throw new ArgumentException("End date cannot be before start date.", nameof(endDate));

        return new Experience
        {
            Company = company.Trim(),
            Position = position.Trim(),
            StartDate = startDate,
            EndDate = endDate,
            Description = description?.Trim(),
            Location = location?.Trim(),
            CompanyUrl = companyUrl?.Trim(),
            EmploymentType = employmentType?.Trim(),
            DisplayOrder = displayOrder
        };
    }

    public void Update(
        string company, string position,
        DateTime startDate, DateTime? endDate,
        string? description, string? location,
        string? companyUrl, string? employmentType, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(company))
            throw new ArgumentException("Company is required.", nameof(company));

        if (string.IsNullOrWhiteSpace(position))
            throw new ArgumentException("Position is required.", nameof(position));

        if (endDate.HasValue && endDate < startDate)
            throw new ArgumentException("End date cannot be before start date.", nameof(endDate));

        Company = company.Trim();
        Position = position.Trim();
        StartDate = startDate;
        EndDate = endDate;
        Description = description?.Trim();
        Location = location?.Trim();
        CompanyUrl = companyUrl?.Trim();
        EmploymentType = employmentType?.Trim();
        DisplayOrder = displayOrder;
        UpdateTimestamp();
    }
}