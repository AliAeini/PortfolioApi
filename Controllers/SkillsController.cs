using Microsoft.AspNetCore.Mvc;
using PortfolioApi.Common;
using PortfolioApi.Interfaces;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/skills")]
[Tags("Skills")]
public class SkillsController : ControllerBase
{
    private readonly ISkillService _service;

    public SkillsController(ISkillService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var skills = await _service.GetAllAsync(ct);
        return ApiResults.Ok(skills, $"{skills.Count()} skill(s) found");
    }

    [HttpGet("grouped")]
    public async Task<IActionResult> GetGrouped(CancellationToken ct)
    {
        var grouped = await _service.GetGroupedAsync(ct);
        return ApiResults.Ok(grouped, $"{grouped.Count()} categor(ies) found");
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(CancellationToken ct)
    {
        var categories = await _service.GetCategoriesAsync(ct);
        return ApiResults.Ok(categories, $"{categories.Count()} categor(ies) found");
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var skill = await _service.GetByIdAsync(id, ct);
        return ApiResults.Ok(skill);
    }
}