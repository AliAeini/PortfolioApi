namespace PortfolioApi.DTOs;

public record ExperienceDto(
    Guid Id,
    Guid ProfileId,
    string Company,
    string Position,
    int EmploymentType,
    string EmploymentTypeName,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description,
    string? Location,
    string? CompanyUrl,
    int DisplayOrder,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateExperienceRequest(
    string Company,
    string Position,
    int EmploymentType,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description,
    string? Location,
    string? CompanyUrl,
    int DisplayOrder = 0
);

public record UpdateExperienceRequest(
    string Company,
    string Position,
    int EmploymentType,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description,
    string? Location,
    string? CompanyUrl,
    int DisplayOrder
);