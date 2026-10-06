using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Infrastructure.Authorization;
using PortfolioApi.Services;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/profiles")]
[Tags("Profiles")]
public class ProfilesController : ControllerBase
{
    private readonly IProfileService _service;

    public ProfilesController(IProfileService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var profiles = await _service.GetAllAsync(ct);
        return ApiResults.Ok(profiles, $"{profiles.Count()} profile(s) found");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var profile = await _service.GetByIdAsync(id, ct);
        return ApiResults.Ok(profile);
    }

    // [HttpPost]
    // public async Task<IActionResult> Create([FromBody] CreateProfileRequest req, CancellationToken ct)
    // {
    //     var profile = await _service.CreateAsync(req, ct);
    //     return ApiResults.Created($"/api/profiles/{profile.Id}", profile, "Profile created successfully");
    // }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyNames.ProfileOwner)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProfileRequest req, CancellationToken ct)
    {
        var profile = await _service.UpdateAsync(id, req, ct);
        return ApiResults.Ok(profile, "Profile updated successfully");
    }

    // [HttpDelete("{id:guid}")]
    // [Authorize(Policy = PolicyNames.ProfileOwner)]
    // public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    // {
    //     await _service.DeleteAsync(id, ct);
    //     return ApiResults.Ok("Profile deleted successfully");
    // }
}