using PortfolioApi.DTOs;
using PortfolioApi.Interfaces;
using PortfolioApi.Models;
using PortfolioApi.Repositories;

namespace PortfolioApi.Services;

public class ProfileSkillService : IProfileSkillService
{
    private readonly IProfileSkillRepository _repository;
    private readonly ISkillRepository _skillRepository;
    private readonly IProfileRepository _profileRepository;

    public ProfileSkillService(
        IProfileSkillRepository repository,
        ISkillRepository skillRepository,
        IProfileRepository profileRepository)
    {
        _repository = repository;
        _skillRepository = skillRepository;
        _profileRepository = profileRepository;
    }

    public async Task<IEnumerable<ProfileSkillDto>> GetByProfileAsync(
        Guid profileId, CancellationToken cancellationToken = default)
    {
        var profileSkills = await _repository.GetByProfileAsync(profileId, cancellationToken);
        return profileSkills.Select(MapToDto);
    }

    public async Task<ProfileSkillDto> AddAsync(
     Guid profileId,
     AddProfileSkillRequest request,
     CancellationToken cancellationToken = default)
    {
        var profile = await _profileRepository.GetByIdAsync(profileId, cancellationToken);
        if (profile is null)
            throw new KeyNotFoundException($"Profile with id '{profileId}' not found");

        var skill = await _skillRepository.GetByIdAsync(request.SkillId, cancellationToken);
        if (skill is null)
            throw new KeyNotFoundException($"Skill with id '{request.SkillId}' not found");

        if (await _repository.ExistsAsync(profileId, request.SkillId, cancellationToken))
            throw new InvalidOperationException("This skill is already added to the profile.");

        var profileSkill = ProfileSkill.Create(
            profileId,
            request.SkillId,
            request.Level,
            request.DisplayOrder);

        await _repository.AddAsync(profileSkill, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        var created = await _repository.GetByProfileAndSkillAsync(
            profileId, request.SkillId, cancellationToken)
            ?? profileSkill;

        return MapToDto(created);
    }

    public async Task<ProfileSkillDto> UpdateAsync(
        Guid profileId,
        Guid profileSkillId,
        UpdateProfileSkillRequest request,
        CancellationToken cancellationToken = default)
    {
        var profileSkill = await _repository.GetByIdWithSkillAsync(profileSkillId, cancellationToken);
        if (profileSkill is null || profileSkill.ProfileId != profileId)
            throw new KeyNotFoundException($"ProfileSkill with id '{profileSkillId}' not found");

        profileSkill.Update(request.Level, request.DisplayOrder);

        await _repository.SaveChangesAsync(cancellationToken);

        return MapToDto(profileSkill);
    }

    public async Task DeleteAsync(
        Guid profileId, Guid profileSkillId, CancellationToken cancellationToken = default)
    {
        var profileSkill = await _repository.GetByIdAsync(profileSkillId, cancellationToken);
        if (profileSkill is null || profileSkill.ProfileId != profileId)
            throw new KeyNotFoundException($"ProfileSkill with id '{profileSkillId}' not found");

        _repository.Remove(profileSkill);
        await _repository.SaveChangesAsync(cancellationToken);
    }

    private static ProfileSkillDto MapToDto(ProfileSkill ps)
        => new(
            ps.Id,
            ps.SkillId,
            ps.Skill?.Name ?? string.Empty,
            ps.Skill?.IconUrl,
            ps.Skill?.SkillCategory?.Name,
            ps.Level,
            ps.DisplayOrder);
}