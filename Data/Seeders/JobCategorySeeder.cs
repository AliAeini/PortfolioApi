using Microsoft.EntityFrameworkCore;
using PortfolioApi.Models;

namespace PortfolioApi.Data.Seeders;

public class JobCategorySeeder : ISeeder
{
    private readonly AppDbContext _db;
    private readonly ILogger<JobCategorySeeder> _logger;

    public JobCategorySeeder(AppDbContext db, ILogger<JobCategorySeeder> logger)
    {
        _db = db;
        _logger = logger;
    }

    public int Order => 25;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _db.JobCategories.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Job categories already exist. Skipping seed.");
            return;
        }

        var categories = new[]
        {
            JobCategory.Create("Frontend Developer", "Frontend engineer", 1),
            JobCategory.Create("Backend Developer", "Backend engineer", 2),
            JobCategory.Create("Full-Stack Developer", "Full-stack engineer", 3),
            JobCategory.Create("Mobile Developer", "iOS/Android developer", 4),
            JobCategory.Create("DevOps Engineer", "DevOps and SRE", 5),
            JobCategory.Create("Data Scientist", "Data science and ML", 6),
            JobCategory.Create("Data Engineer", "Data pipelines and ETL", 7),
            JobCategory.Create("UI/UX Designer", "User interface and experience design", 8),
            JobCategory.Create("QA Engineer", "Quality assurance", 9),
            JobCategory.Create("Product Manager", "Product management", 10),
            JobCategory.Create("Software Architect", "Software architecture", 11),
            JobCategory.Create("Tech Lead", "Technical leadership", 12),
            JobCategory.Create("Engineering Manager", "Engineering management", 13),
            JobCategory.Create("Security Engineer", "Security and compliance", 14),
            JobCategory.Create("Cloud Engineer", "Cloud platforms and infrastructure", 15),
        };

        _db.JobCategories.AddRange(categories);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Seeded {Count} job categories.", categories.Length);
    }
}