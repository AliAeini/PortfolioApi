using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Infrastructure.Authorization;
using PortfolioApi.Interfaces;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/profiles/{profileId:guid}/social-links")]
[Tags("Social Links")]
public class SocialLinksController : ControllerBase
{
    private readonly ISocialLinkService _service;

    public SocialLinksController(ISocialLinkService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid profileId, CancellationToken ct)
    {
        var links = await _service.GetByProfileAsync(profileId, ct);
        return ApiResults.Ok(links, $"{links.Count()} social link(s) found");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid profileId, Guid id, CancellationToken ct)
    {
        var link = await _service.GetByIdAsync(id, ct);
        return ApiResults.Ok(link);
    }

    [HttpPost]
    [Authorize(Policy = PolicyNames.ProfileOwner)]
    public async Task<IActionResult> Create(
        Guid profileId,
        [FromBody] CreateSocialLinkRequest req,
        CancellationToken ct)
    {
        var link = await _service.CreateAsync(profileId, req, ct);
        return ApiResults.Created(
            $"/api/profiles/{profileId}/social-links/{link.Id}",
            link,
            "Social link created successfully");
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyNames.ProfileOwner)]
    public async Task<IActionResult> Update(
        Guid profileId,
        Guid id,
        [FromBody] UpdateSocialLinkRequest req,
        CancellationToken ct)
    {
        var link = await _service.UpdateAsync(profileId, id, req, ct);
        return ApiResults.Ok(link, "Social link updated successfully");
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyNames.ProfileOwner)]
    public async Task<IActionResult> Delete(Guid profileId, Guid id, CancellationToken ct)
    {
        await _service.DeleteAsync(profileId, id, ct);
        return ApiResults.Ok("Social link deleted successfully");
    }
}