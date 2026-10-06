using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Common;
using PortfolioApi.DTOs;
using PortfolioApi.Infrastructure.Authorization;
using PortfolioApi.Interfaces;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/profiles/{profileId:guid}/projects")]
[Tags("Projects")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _service;

    public ProjectsController(IProjectService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(Guid profileId, CancellationToken ct)
    {
        var projects = await _service.GetByProfileAsync(profileId, ct);
        return ApiResults.Ok(projects, $"{projects.Count()} project(s) found");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid profileId, Guid id, CancellationToken ct)
    {
        var project = await _service.GetByIdAsync(id, ct);
        return ApiResults.Ok(project);
    }

    [HttpPost]
    [Authorize(Policy = PolicyNames.ProfileOwner)]
    public async Task<IActionResult> Create(
        Guid profileId,
        [FromBody] CreateProjectRequest req,
        CancellationToken ct)
    {
        var project = await _service.CreateAsync(profileId, req, ct);
        return ApiResults.Created(
            $"/api/profiles/{profileId}/projects/{project.Id}",
            project,
            "Project created successfully");
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = PolicyNames.ProfileOwner)]
    public async Task<IActionResult> Update(
        Guid profileId,
        Guid id,
        [FromBody] UpdateProjectRequest req,
        CancellationToken ct)
    {
        var project = await _service.UpdateAsync(profileId, id, req, ct);
        return ApiResults.Ok(project, "Project updated successfully");
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = PolicyNames.ProfileOwner)]
    public async Task<IActionResult> Delete(Guid profileId, Guid id, CancellationToken ct)
    {
        await _service.DeleteAsync(profileId, id, ct);
        return ApiResults.Ok("Project deleted successfully");
    }
}