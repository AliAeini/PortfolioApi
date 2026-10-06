using PortfolioApi.Common;

namespace PortfolioApi.Models;

public class Education : BaseEntity
{
    public Guid ProfileId { get; private set; }
    public Profile Profile { get; private set; } = null!;
    public string Institution { get; private set; } = null!;
    public DegreeLevel Degree { get; private set; }
    public string Field { get; private set; } = null!;
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string? Description { get; private set; }
    public string? Location { get; private set; }
    public string? Grade { get; private set; }
    public int DisplayOrder { get; private set; }

    private Education() { }

    public static Education Create(
        Guid profileId,
        string institution,
        DegreeLevel degree,
        string field,
        DateTime startDate,
        DateTime? endDate = null,
        string? description = null,
        string? location = null,
        string? grade = null,
        int displayOrder = 0)
    {
        if (profileId == Guid.Empty)
            throw new ArgumentException("Profile ID is required.", nameof(profileId));

        if (string.IsNullOrWhiteSpace(institution))
            throw new ArgumentException("Institution is required.", nameof(institution));

        if (institution.Length > 200)
            throw new ArgumentException("Institution must not exceed 200 characters.", nameof(institution));

        if (!Enum.IsDefined(typeof(DegreeLevel), degree))
            throw new ArgumentException("Invalid degree level.", nameof(degree));

        if (string.IsNullOrWhiteSpace(field))
            throw new ArgumentException("Field is required.", nameof(field));

        if (field.Length > 200)
            throw new ArgumentException("Field must not exceed 200 characters.", nameof(field));

        if (startDate == default)
            throw new ArgumentException("Start date is required.", nameof(startDate));

        if (endDate.HasValue && endDate.Value < startDate)
            throw new ArgumentException("End date cannot be before start date.", nameof(endDate));

        if (!string.IsNullOrWhiteSpace(description) && description.Length > 2000)
            throw new ArgumentException("Description must not exceed 2000 characters.", nameof(description));

        if (!string.IsNullOrWhiteSpace(location) && location.Length > 200)
            throw new ArgumentException("Location must not exceed 200 characters.", nameof(location));

        if (!string.IsNullOrWhiteSpace(grade) && grade.Length > 50)
            throw new ArgumentException("Grade must not exceed 50 characters.", nameof(grade));

        return new Education
        {
            ProfileId = profileId,
            Institution = institution.Trim(),
            Degree = degree,
            Field = field.Trim(),
            StartDate = startDate,
            EndDate = endDate,
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim(),
            Grade = string.IsNullOrWhiteSpace(grade) ? null : grade.Trim(),
            DisplayOrder = displayOrder,
            Status = EntityStatus.Active
        };
    }

    public void Update(
        string institution,
        DegreeLevel degree,
        string field,
        DateTime startDate,
        DateTime? endDate,
        string? description,
        string? location,
        string? grade,
        int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(institution))
            throw new ArgumentException("Institution is required.", nameof(institution));

        if (!Enum.IsDefined(typeof(DegreeLevel), degree))
            throw new ArgumentException("Invalid degree level.", nameof(degree));

        if (string.IsNullOrWhiteSpace(field))
            throw new ArgumentException("Field is required.", nameof(field));

        if (startDate == default)
            throw new ArgumentException("Start date is required.", nameof(startDate));

        if (endDate.HasValue && endDate.Value < startDate)
            throw new ArgumentException("End date cannot be before start date.", nameof(endDate));

        Institution = institution.Trim();
        Degree = degree;
        Field = field.Trim();
        StartDate = startDate;
        EndDate = endDate;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Location = string.IsNullOrWhiteSpace(location) ? null : location.Trim();
        Grade = string.IsNullOrWhiteSpace(grade) ? null : grade.Trim();
        DisplayOrder = displayOrder;
    }
}