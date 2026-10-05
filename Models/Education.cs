using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class Education : BaseEntity
{
    public string Institution { get; private set; } = null!;
    public string Degree { get; private set; } = null!;
    public string Field { get; private set; } = null!;
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string? Description { get; private set; }
    public string? Location { get; private set; }
    public string? Grade { get; private set; }
    public int DisplayOrder { get; private set; }

    private Education() { }

    public static Education Create(
        string institution,
        string degree,
        string field,
        DateTime startDate,
        DateTime? endDate = null,
        string? description = null,
        string? location = null,
        string? grade = null,
        int displayOrder = 0)
    {
        if (string.IsNullOrWhiteSpace(institution))
            throw new ArgumentException("Institution is required.", nameof(institution));

        if (string.IsNullOrWhiteSpace(degree))
            throw new ArgumentException("Degree is required.", nameof(degree));

        if (string.IsNullOrWhiteSpace(field))
            throw new ArgumentException("Field is required.", nameof(field));

        if (endDate.HasValue && endDate < startDate)
            throw new ArgumentException("End date cannot be before start date.", nameof(endDate));

        return new Education
        {
            Institution = institution.Trim(),
            Degree = degree.Trim(),
            Field = field.Trim(),
            StartDate = startDate,
            EndDate = endDate,
            Description = description?.Trim(),
            Location = location?.Trim(),
            Grade = grade?.Trim(),
            DisplayOrder = displayOrder
        };
    }

    public void Update(
        string institution, string degree, string field,
        DateTime startDate, DateTime? endDate,
        string? description, string? location, string? grade, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(institution))
            throw new ArgumentException("Institution is required.", nameof(institution));

        if (string.IsNullOrWhiteSpace(degree))
            throw new ArgumentException("Degree is required.", nameof(degree));

        if (string.IsNullOrWhiteSpace(field))
            throw new ArgumentException("Field is required.", nameof(field));

        if (endDate.HasValue && endDate < startDate)
            throw new ArgumentException("End date cannot be before start date.", nameof(endDate));

        Institution = institution.Trim();
        Degree = degree.Trim();
        Field = field.Trim();
        StartDate = startDate;
        EndDate = endDate;
        Description = description?.Trim();
        Location = location?.Trim();
        Grade = grade?.Trim();
        DisplayOrder = displayOrder;
        UpdateTimestamp();
    }
}