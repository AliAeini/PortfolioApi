namespace PortfolioApi.DTOs;

public record SkillDto(
    Guid Id,
    string Name,
    int Level,
    int DisplayOrder,
    string? IconUrl,
    SkillCategoryDto? Category
);

public record SkillCategoryDto(
    Guid Id,
    string Name,
    string? Description,
    int DisplayOrder,
    int SkillCount
);

public record GroupedSkillsDto(
    Guid CategoryId,
    string CategoryName,
    string? CategoryDescription,
    int CategoryDisplayOrder,
    List<SkillItemDto> Skills
);

public record SkillItemDto(
    Guid Id,
    string Name,
    int Level,
    int DisplayOrder,
    string? IconUrl
);

public record ProfileSkillDto(
    Guid Id,
    Guid SkillId,
    string SkillName,
    string? SkillIconUrl,
    string? CategoryName,
    int Level,
    int DisplayOrder
);

public record AddProfileSkillRequest(
    Guid SkillId,
    int Level = 50,
    int DisplayOrder = 0
);

public record UpdateProfileSkillRequest(
    int Level,
    int DisplayOrder = 0
);