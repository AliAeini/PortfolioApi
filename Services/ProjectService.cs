using PortfolioApi.DTOs;
using PortfolioApi.Interfaces;
using PortfolioApi.Models;
using PortfolioApi.Repositories;

namespace PortfolioApi.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;
    private readonly IProfileRepository _profileRepository;
    private readonly IFileStorageService _fileStorage;

    public ProjectService(
        IProjectRepository repository,
        IProfileRepository profileRepository,
        IFileStorageService fileStorage)
    {
        _repository = repository;
        _profileRepository = profileRepository;
        _fileStorage = fileStorage;
    }

    public async Task<IEnumerable<ProjectDto>> GetByProfileAsync(
        Guid profileId, CancellationToken cancellationToken = default)
    {
        var projects = await _repository.GetByProfileAsync(profileId, cancellationToken);
        return projects.Select(MapToDto);
    }

    public async Task<ProjectDto> GetByIdAsync(
        Guid id, CancellationToken cancellationToken = default)
    {
        var project = await _repository.GetByIdWithDetailsAsync(id, cancellationToken);
        if (project is null)
            throw new KeyNotFoundException($"Project with id '{id}' not found");

        return MapToDto(project);
    }

    public async Task<ProjectDto> CreateAsync(
        Guid profileId,
        CreateProjectRequest request,
        CancellationToken cancellationToken = default)
    {
        var profile = await _profileRepository.GetByIdAsync(profileId, cancellationToken);
        if (profile is null)
            throw new KeyNotFoundException($"Profile with id '{profileId}' not found");

        var project = Project.Create(
            profileId,
            request.Title,
            request.Description,
            request.ProjectCategoryId,
            request.ShortDescription,
            request.GithubUrl,
            request.LiveUrl,
            request.StartDate,
            request.EndDate,
            request.IsFeatured,
            request.DisplayOrder);

        foreach (var profileSkillId in request.ProfileSkillIds.Distinct())
        {
            project.AddProfileSkill(profileSkillId);
        }

        foreach (var img in request.Images)
        {
            project.Images.Add(ProjectImage.Create(
                project.Id,
                img.ImageUrl,
                img.Caption,
                img.IsCover,
                img.DisplayOrder));
        }

        await _repository.AddAsync(project, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        var created = await _repository.GetByIdWithDetailsAsync(project.Id, cancellationToken) ?? project;

        return MapToDto(created);
    }

    public async Task DeleteAsync(
        Guid profileId, Guid projectId, CancellationToken cancellationToken = default)
    {
        var project = await _repository.GetByIdWithDetailsAsync(projectId, cancellationToken);
        if (project is null || project.ProfileId != profileId)
            throw new KeyNotFoundException($"Project with id '{projectId}' not found");

        foreach (var img in project.Images)
        {
            if (!string.IsNullOrWhiteSpace(img.ImageUrl))
                _fileStorage.DeleteFile(img.ImageUrl);
        }

        _repository.Remove(project);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    public async Task<ProjectDto> UpdateAsync(
       Guid profileId,
       Guid projectId,
       UpdateProjectRequest request,
       CancellationToken cancellationToken = default)
    {
        var project = await _repository.GetByIdWithDetailsAsync(projectId, cancellationToken);
        if (project is null || project.ProfileId != profileId)
            throw new KeyNotFoundException($"Project with id '{projectId}' not found");

        project.Update(
            request.Title,
            request.Description,
            request.ProjectCategoryId,
            request.ShortDescription,
            request.GithubUrl,
            request.LiveUrl,
            request.StartDate,
            request.EndDate,
            request.IsFeatured,
            request.DisplayOrder);

        project.ProjectSkills.Clear();

        var newSkillRefs = request.ProfileSkillIds
            .Distinct()
            .Select(profileSkillId => ProjectSkillRef.Create(project.Id, profileSkillId))
            .ToList();

        project.ProjectSkills.AddRange(newSkillRefs);

        var incomingImageUrls = request.Images
            .Where(img => !string.IsNullOrWhiteSpace(img.ImageUrl))
            .Select(img => img.ImageUrl!)
            .ToHashSet();

        var filesToDelete = project.Images
            .Where(img => !string.IsNullOrWhiteSpace(img.ImageUrl))
            .Select(img => img.ImageUrl!)
            .Where(url => !incomingImageUrls.Contains(url))
            .ToList();

        project.Images.Clear();

        var newImages = request.Images
            .Where(img => !string.IsNullOrWhiteSpace(img.ImageUrl))
            .Select(img => ProjectImage.Create(
                project.Id,
                img.ImageUrl!,
                img.Caption,
                img.IsCover,
                img.DisplayOrder))
            .ToList();

        project.Images.AddRange(newImages);

        await _repository.SaveChangesAsync(cancellationToken);

        foreach (var filePath in filesToDelete)
        {
            try { _fileStorage.DeleteFile(filePath); }
            catch
            {

            }
        }

        return MapToDto(project);
    }

    private static ProjectDto MapToDto(Project p)
        => new(
            p.Id,
            p.ProfileId,
            p.ProjectCategoryId,
            p.ProjectCategory?.Name,
            p.Title,
            p.Description,
            p.ShortDescription,
            p.GithubUrl,
            p.LiveUrl,
            p.StartDate,
            p.EndDate,
            p.IsFeatured,
            p.DisplayOrder,
            p.ProjectSkills
                .OrderBy(ps => ps.DisplayOrder)
                .Select(ps => new ProjectSkillDto(
                    ps.Id,
                    ps.ProfileSkillId,
                    ps.ProfileSkill?.SkillId ?? Guid.Empty,
                    ps.ProfileSkill?.Skill?.Name ?? string.Empty,
                    ps.ProfileSkill?.Skill?.IconUrl,
                    ps.ProfileSkill?.Skill?.SkillCategory?.Name,
                    ps.ProfileSkill?.Level ?? 0,
                    ps.DisplayOrder))
                .ToList(),
            p.Images
                .OrderByDescending(i => i.IsCover)
                .ThenBy(i => i.DisplayOrder)
                .Select(i => new ProjectImageDto(
                    i.Id,
                    i.ImageUrl,
                    i.Caption,
                    i.IsCover,
                    i.DisplayOrder))
                .ToList(),
            p.CreatedAt,
            p.UpdatedAt);
}