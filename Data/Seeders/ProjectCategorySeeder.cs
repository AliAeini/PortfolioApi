using Microsoft.EntityFrameworkCore;
using PortfolioApi.Models;

namespace PortfolioApi.Data.Seeders;

public class ProjectCategorySeeder : ISeeder
{
    private readonly AppDbContext _db;
    private readonly ILogger<ProjectCategorySeeder> _logger;

    public ProjectCategorySeeder(AppDbContext db, ILogger<ProjectCategorySeeder> logger)
    {
        _db = db;
        _logger = logger;
    }

    public int Order => 20;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _db.ProjectCategories.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Project categories already exist. Skipping seed.");
            return;
        }

        var categories = new[]
        {
            ProjectCategory.Create("Web Application", "Web-based applications", 1),
            ProjectCategory.Create("Mobile Application", "Mobile applications", 2),
            ProjectCategory.Create("API / Backend", "APIs and backend services", 3),
            ProjectCategory.Create("Desktop Application", "Desktop software", 4),
            ProjectCategory.Create("Open Source", "Open source contributions", 5),
            ProjectCategory.Create("Personal", "Personal projects", 6),
            ProjectCategory.Create("Freelance", "Client and freelance projects", 7),
            ProjectCategory.Create("Educational", "Learning and educational projects", 8)
        };

        _db.ProjectCategories.AddRange(categories);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Seeded {Count} project categories.", categories.Length);
    }
}