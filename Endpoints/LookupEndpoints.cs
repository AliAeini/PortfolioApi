using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.Data;

namespace PortfolioApi.Endpoints;

public static class LookupEndpoints
{
    public static void MapLookupEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/lookups").WithTags("Lookups");

        group.MapGet("/skill-categories", async (AppDbContext db, CancellationToken ct) =>
        {
            var categories = await db.SkillCategories
                .OrderBy(c => c.DisplayOrder)
                .Select(c => new { c.Id, c.Name, c.Description, c.DisplayOrder })
                .ToListAsync(ct);

            return ApiResults.Ok(categories, $"{categories.Count} skill categories found");
        });

        group.MapGet("/project-categories", async (AppDbContext db, CancellationToken ct) =>
        {
            var categories = await db.ProjectCategories
                .OrderBy(c => c.DisplayOrder)
                .Select(c => new { c.Id, c.Name, c.Description, c.DisplayOrder })
                .ToListAsync(ct);

            return ApiResults.Ok(categories, $"{categories.Count} project categories found");
        });

        group.MapGet("/job-categories", async (AppDbContext db, CancellationToken ct) =>
               {
                   var categories = await db.JobCategories
                       .OrderBy(c => c.DisplayOrder)
                       .Select(c => new { c.Id, c.Name, c.Description, c.DisplayOrder })
                       .ToListAsync(ct);

                   return ApiResults.Ok(categories, $"{categories.Count} job categories found");
               });

        group.MapGet("/degree-levels", () =>
            {
                var degrees = Enum.GetValues<DegreeLevel>()
                .Select(d => new
                {
                    Value = (int)d,
                    Name = d.ToString()
                })
                .ToList();

                return ApiResults.Ok(degrees, $"{degrees.Count} degree levels found");
            });
    }
}