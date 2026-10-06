using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Infrastructure.Authorization;
using PortfolioApi.Interfaces;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/profiles/{profileId:guid}/educations")]
[Tags("Educations")]
public class EducationsController : ControllerBase
{
    private readonly IEducationService _service;

    public EducationsController(IEducationService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid profileId, CancellationToken ct)
    {
        var educations = await _service.GetByProfileAsync(profileId, ct);
        return ApiResults.Ok(educations, $"{educations.Count()} education(s) found");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid profileId, Guid id, CancellationToken ct)
    {
        var education = await _service.GetByIdAsync(id, ct);
        return ApiResults.Ok(education);
    }

    [HttpPost]
    [Authorize(Policy = PolicyNames.ProfileOwner)]
    public async Task<IActionResult> Create(
        Guid profileId,
        [FromBody] CreateEducationRequest req,
        CancellationToken ct)
    {
        var education = await _service.CreateAsync(profileId, req, ct);
        return ApiResults.Created(
            $"/api/profiles/{profileId}/educations/{education.Id}",
            education,
            "Education created successfully");
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyNames.ProfileOwner)]
    public async Task<IActionResult> Update(
        Guid profileId,
        Guid id,
        [FromBody] UpdateEducationRequest req,
        CancellationToken ct)
    {
        var education = await _service.UpdateAsync(profileId, id, req, ct);
        return ApiResults.Ok(education, "Education updated successfully");
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyNames.ProfileOwner)]
    public async Task<IActionResult> Delete(Guid profileId, Guid id, CancellationToken ct)
    {
        await _service.DeleteAsync(profileId, id, ct);
        return ApiResults.Ok("Education deleted successfully");
    }
}