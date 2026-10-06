namespace PortfolioApi.DTOs;

public record ProjectDto(
    Guid Id,
    Guid ProfileId,
    Guid? ProjectCategoryId,
    string? ProjectCategoryName,
    string Title,
    string Description,
    string? ShortDescription,
    string? GithubUrl,
    string? LiveUrl,
    DateTime? StartDate,
    DateTime? EndDate,
    bool IsFeatured,
    int DisplayOrder,
    List<ProjectSkillDto> Skills,
    List<ProjectImageDto> Images,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public record ProjectSkillDto(
    Guid Id,
    Guid ProfileSkillId,               
    Guid SkillId,
    string SkillName,
    string? SkillIconUrl,
    string? CategoryName,
    int Level,
    int DisplayOrder
);

public record ProjectImageDto(
    Guid Id,
    string ImageUrl,
    string? Caption,
    bool IsCover,
    int DisplayOrder
);

public record CreateProjectRequest(
    string Title,
    string Description,
    Guid? ProjectCategoryId,
    string? ShortDescription,
    string? GithubUrl,
    string? LiveUrl,
    DateTime? StartDate,
    DateTime? EndDate,
    bool IsFeatured,
    int DisplayOrder,
    List<Guid> ProfileSkillIds,        
    List<CreateProjectImageRequest> Images
);

public record CreateProjectImageRequest(
    string ImageUrl,
    string? Caption,
    bool IsCover,
    int DisplayOrder
);

public record UpdateProjectRequest(
    string Title,
    string Description,
    Guid? ProjectCategoryId,
    string? ShortDescription,
    string? GithubUrl,
    string? LiveUrl,
    DateTime? StartDate,
    DateTime? EndDate,
    bool IsFeatured,
    int DisplayOrder,
    List<Guid> ProfileSkillIds,
    List<UpdateProjectImageRequest> Images
);

public record UpdateProjectImageRequest(
    Guid? Id,
    string ImageUrl,
    string? Caption,
    bool IsCover,
    int DisplayOrder
);