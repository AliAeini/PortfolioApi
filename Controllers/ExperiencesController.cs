using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Infrastructure.Authorization;
using PortfolioApi.Interfaces;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/profiles/{profileId:guid}/experiences")]
[Tags("Experiences")]
public class ExperiencesController : ControllerBase
{
    private readonly IExperienceService _service;

    public ExperiencesController(IExperienceService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid profileId, CancellationToken ct)
    {
        var experiences = await _service.GetByProfileAsync(profileId, ct);
        return ApiResults.Ok(experiences, $"{experiences.Count()} experience(s) found");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid profileId, Guid id, CancellationToken ct)
    {
        var experience = await _service.GetByIdAsync(id, ct);
        return ApiResults.Ok(experience);
    }

    [HttpPost]
    [Authorize(Policy = PolicyNames.ProfileOwner)]
    public async Task<IActionResult> Create(
        Guid profileId,
        [FromBody] CreateExperienceRequest req,
        CancellationToken ct)
    {
        var experience = await _service.CreateAsync(profileId, req, ct);
        return ApiResults.Created(
            $"/api/profiles/{profileId}/experiences/{experience.Id}",
            experience,
            "Experience created successfully");
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyNames.ProfileOwner)]
    public async Task<IActionResult> Update(
        Guid profileId,
        Guid id,
        [FromBody] UpdateExperienceRequest req,
        CancellationToken ct)
    {
        var experience = await _service.UpdateAsync(profileId, id, req, ct);
        return ApiResults.Ok(experience, "Experience updated successfully");
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyNames.ProfileOwner)]
    public async Task<IActionResult> Delete(Guid profileId, Guid id, CancellationToken ct)
    {
        await _service.DeleteAsync(profileId, id, ct);
        return ApiResults.Ok("Experience deleted successfully");
    }
}