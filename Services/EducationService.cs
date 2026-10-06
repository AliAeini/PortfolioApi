using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Interfaces;
using PortfolioApi.Models;
using PortfolioApi.Repositories;

namespace PortfolioApi.Services;

public class EducationService : IEducationService
{
    private readonly IEducationRepository _repository;
    private readonly IProfileRepository _profileRepository;

    public EducationService(
        IEducationRepository repository,
        IProfileRepository profileRepository)
    {
        _repository = repository;
        _profileRepository = profileRepository;
    }

    public async Task<IEnumerable<EducationDto>> GetByProfileAsync(
        Guid profileId, CancellationToken cancellationToken = default)
    {
        var educations = await _repository.GetByProfileAsync(profileId, cancellationToken);
        return educations.Select(MapToDto);
    }

    public async Task<EducationDto> GetByIdAsync(
        Guid id, CancellationToken cancellationToken = default)
    {
        var education = await _repository.GetByIdAsync(id, cancellationToken);
        if (education is null)
            throw new KeyNotFoundException($"Education with id '{id}' not found");

        return MapToDto(education);
    }

    public async Task<EducationDto> CreateAsync(
        Guid profileId,
        CreateEducationRequest request,
        CancellationToken cancellationToken = default)
    {
        var profile = await _profileRepository.GetByIdAsync(profileId, cancellationToken);
        if (profile is null)
            throw new KeyNotFoundException($"Profile with id '{profileId}' not found");

        if (!Enum.IsDefined(typeof(DegreeLevel), request.Degree))
            throw new ArgumentException("Invalid degree level.", nameof(request.Degree));

        var degree = (DegreeLevel)request.Degree;

        var education = Education.Create(
            profileId,
            request.Institution,
            degree,
            request.Field,
            request.StartDate,
            request.EndDate,
            request.Description,
            request.Location,
            request.Grade,
            request.DisplayOrder);

        await _repository.AddAsync(education, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return MapToDto(education);
    }

    public async Task<EducationDto> UpdateAsync(
        Guid profileId,
        Guid educationId,
        UpdateEducationRequest request,
        CancellationToken cancellationToken = default)
    {
        var education = await _repository.GetByIdAsync(educationId, cancellationToken);
        if (education is null || education.ProfileId != profileId)
            throw new KeyNotFoundException($"Education with id '{educationId}' not found");

        if (!Enum.IsDefined(typeof(DegreeLevel), request.Degree))
            throw new ArgumentException("Invalid degree level.", nameof(request.Degree));

        var degree = (DegreeLevel)request.Degree;

        education.Update(
            request.Institution,
            degree,
            request.Field,
            request.StartDate,
            request.EndDate,
            request.Description,
            request.Location,
            request.Grade,
            request.DisplayOrder);

        await _repository.SaveChangesAsync(cancellationToken);

        return MapToDto(education);
    }

    public async Task DeleteAsync(
        Guid profileId,
        Guid educationId,
        CancellationToken cancellationToken = default)
    {
        var education = await _repository.GetByIdAsync(educationId, cancellationToken);
        if (education is null || education.ProfileId != profileId)
            throw new KeyNotFoundException($"Education with id '{educationId}' not found");

        _repository.Remove(education);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private static EducationDto MapToDto(Education e)
        => new(
            e.Id,
            e.ProfileId,
            e.Institution,
            (int)e.Degree,
            e.Degree.ToString(),
            e.Field,
            e.StartDate,
            e.EndDate,
            e.Description,
            e.Location,
            e.Grade,
            e.DisplayOrder,
            e.CreatedAt,
            e.UpdatedAt);
}