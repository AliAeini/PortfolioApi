namespace PortfolioApi.DTOs;

public record EducationDto(
    Guid Id,
    Guid ProfileId,
    string Institution,
    int Degree,
    string DegreeName,
    string Field,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description,
    string? Location,
    string? Grade,
    int DisplayOrder,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record CreateEducationRequest(
    string Institution,
    int Degree,
    string Field,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description,
    string? Location,
    string? Grade,
    int DisplayOrder = 0
);

public record UpdateEducationRequest(
    string Institution,
    int Degree,
    string Field,
    DateTime StartDate,
    DateTime? EndDate,
    string? Description,
    string? Location,
    string? Grade,
    int DisplayOrder
);