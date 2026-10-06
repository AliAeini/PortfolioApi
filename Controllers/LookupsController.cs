using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.Data;

namespace PortfolioApi.Controllers;

[ApiController]
[Route("api/lookups")]
[Tags("Lookups")]
public class LookupsController : ControllerBase
{
    private readonly AppDbContext _db;

    public LookupsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("skill-categories")]
    public async Task<IActionResult> GetSkillCategories(CancellationToken ct)
    {
        var categories = await _db.SkillCategories
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new { c.Id, c.Name, c.Description, c.DisplayOrder })
            .ToListAsync(ct);

        return ApiResults.Ok(categories, $"{categories.Count} skill categories found");
    }

    [HttpGet("project-categories")]
    public async Task<IActionResult> GetProjectCategories(CancellationToken ct)
    {
        var categories = await _db.ProjectCategories
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new { c.Id, c.Name, c.Description, c.DisplayOrder })
            .ToListAsync(ct);

        return ApiResults.Ok(categories, $"{categories.Count} project categories found");
    }

    [HttpGet("job-categories")]
    public async Task<IActionResult> GetJobCategories(CancellationToken ct)
    {
        var categories = await _db.JobCategories
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new { c.Id, c.Name, c.Description, c.DisplayOrder })
            .ToListAsync(ct);

        return ApiResults.Ok(categories, $"{categories.Count} job categories found");
    }

    [HttpGet("degree-levels")]
    public IActionResult GetDegreeLevels()
    {
        var degrees = Enum.GetValues<Common.DegreeLevel>()
            .Select(d => new { Value = (int)d, Name = d.ToString() })
            .ToList();

        return ApiResults.Ok(degrees, $"{degrees.Count} degree levels found");
    }

    [HttpGet("employment-types")]
    public IActionResult GetEmploymentTypes()
    {
        var types = Enum.GetValues<Common.EmploymentType>()
            .Select(t => new { Value = (int)t, Name = t.ToString() })
            .ToList();

        return ApiResults.Ok(types, $"{types.Count} employment types found");
    }

    [HttpGet("social-platforms")]
    public IActionResult GetSocialPlatforms()
    {
        var platforms = Enum.GetValues<Common.SocialPlatform>()
            .Select(p => new { Value = (int)p, Name = p.ToString() })
            .ToList();

        return ApiResults.Ok(platforms, $"{platforms.Count} social platforms found");
    }
}