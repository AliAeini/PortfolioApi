using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Infrastructure.Authorization;
using PortfolioApi.Interfaces;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/profiles/{profileId:guid}/skills")]
[Tags("Profile Skills")]
public class ProfileSkillsController : ControllerBase
{
    private readonly IProfileSkillService _service;

    public ProfileSkillsController(IProfileSkillService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid profileId, CancellationToken ct)
    {
        var skills = await _service.GetByProfileAsync(profileId, ct);
        return ApiResults.Ok(skills, $"{skills.Count()} skill(s) found");
    }

    [HttpPost]
    [Authorize(Policy = PolicyNames.ProfileOwner)]
    public async Task<IActionResult> Add(
        Guid profileId,
        [FromBody] AddProfileSkillRequest req,
        CancellationToken ct)
    {
        var profileSkill = await _service.AddAsync(profileId, req, ct);
        return ApiResults.Created(
            $"/api/profiles/{profileId}/skills/{profileSkill.Id}",
            profileSkill,
            "Skill added to profile successfully");
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyNames.ProfileOwner)]
    public async Task<IActionResult> Update(
        Guid profileId,
        Guid id,
        [FromBody] UpdateProfileSkillRequest req,
        CancellationToken ct)
    {
        var profileSkill = await _service.UpdateAsync(profileId, id, req, ct);
        return ApiResults.Ok(profileSkill, "Profile skill updated successfully");
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyNames.ProfileOwner)]
    public async Task<IActionResult> Delete(Guid profileId, Guid id, CancellationToken ct)
    {
        await _service.DeleteAsync(profileId, id, ct);
        return ApiResults.Ok("Skill removed from profile successfully");
    }
}