using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Interfaces;
using PortfolioApi.Models;
using PortfolioApi.Repositories;

namespace PortfolioApi.Services;

public class ExperienceService : IExperienceService
{
    private readonly IExperienceRepository _repository;
    private readonly IProfileRepository _profileRepository;
    private readonly ICurrentUserService _currentUser;

    public ExperienceService(
        IExperienceRepository repository,
        IProfileRepository profileRepository,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _profileRepository = profileRepository;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<ExperienceDto>> GetByProfileAsync(
        Guid profileId, CancellationToken cancellationToken = default)
    {
        var experiences = await _repository.GetByProfileAsync(profileId, cancellationToken);
        return experiences.Select(MapToDto);
    }

    public async Task<ExperienceDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var experience = await _repository.GetByIdAsync(id, cancellationToken);
        if (experience is null)
            throw new KeyNotFoundException($"Experience with id '{id}' not found");

        return MapToDto(experience);
    }

    public async Task<ExperienceDto> CreateAsync(
        Guid profileId,
        CreateExperienceRequest request,
        CancellationToken cancellationToken = default)
    {
        var profile = await _profileRepository.GetByIdAsync(profileId, cancellationToken);
        if (profile is null)
            throw new KeyNotFoundException($"Profile with id '{profileId}' not found");

        if (!Enum.IsDefined(typeof(EmploymentType), request.EmploymentType))
            throw new ArgumentException("Invalid employment type.", nameof(request.EmploymentType));

        var employmentType = (EmploymentType)request.EmploymentType;

        var experience = Experience.Create(
            profileId,
            request.Company,
            request.Position,
            employmentType,
            request.StartDate,
            request.EndDate,
            request.Description,
            request.Location,
            request.CompanyUrl,
            request.DisplayOrder);

        await _repository.AddAsync(experience, cancellationToken);
        experience.SetCreatedBy(_currentUser.UserId);
        await _repository.SaveChangesAsync(cancellationToken);

        return MapToDto(experience);
    }

    public async Task<ExperienceDto> UpdateAsync(
        Guid profileId,
        Guid experienceId,
        UpdateExperienceRequest request,
        CancellationToken cancellationToken = default)
    {
        var experience = await _repository.GetByIdAsync(experienceId, cancellationToken);
        if (experience is null || experience.ProfileId != profileId)
            throw new KeyNotFoundException($"Experience with id '{experienceId}' not found");

        if (!Enum.IsDefined(typeof(EmploymentType), request.EmploymentType))
            throw new ArgumentException("Invalid employment type.", nameof(request.EmploymentType));

        var employmentType = (EmploymentType)request.EmploymentType;

        experience.Update(
            request.Company,
            request.Position,
            employmentType,
            request.StartDate,
            request.EndDate,
            request.Description,
            request.Location,
            request.CompanyUrl,
            request.DisplayOrder);

        await _repository.SaveChangesAsync(cancellationToken);

        return MapToDto(experience);
    }

    public async Task DeleteAsync(
        Guid profileId, Guid experienceId, CancellationToken cancellationToken = default)
    {
        var experience = await _repository.GetByIdAsync(experienceId, cancellationToken);
        if (experience is null || experience.ProfileId != profileId)
            throw new KeyNotFoundException($"Experience with id '{experienceId}' not found");

        _repository.Remove(experience);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private static ExperienceDto MapToDto(Experience e)
        => new(
            e.Id,
            e.ProfileId,
            e.Company,
            e.Position,
            (int)e.EmploymentType,
            e.EmploymentType.ToString(),
            e.StartDate,
            e.EndDate,
            e.Description,
            e.Location,
            e.CompanyUrl,
            e.DisplayOrder,
            e.CreatedAt,
            e.UpdatedAt);
}