using Microsoft.EntityFrameworkCore;
using PortfolioApi.Models;

namespace PortfolioApi.Data.Seeders;

public class SkillCategorySeeder : ISeeder
{
    public static readonly Guid FrontendId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public static readonly Guid BackendId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid DatabaseId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid DevOpsId = Guid.Parse("44444444-4444-4444-4444-444444444444");
    public static readonly Guid MobileId = Guid.Parse("55555555-5555-5555-5555-555555555555");
    public static readonly Guid ToolsId = Guid.Parse("66666666-6666-6666-6666-666666666666");
    public static readonly Guid SoftSkillsId = Guid.Parse("77777777-7777-7777-7777-777777777777");
    public static readonly Guid TestingId = Guid.Parse("88888888-8888-8888-8888-888888888888");
    public static readonly Guid SecurityId = Guid.Parse("99999999-9999-9999-9999-999999999999");
    public static readonly Guid CloudId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    private readonly AppDbContext _db;
    private readonly ILogger<SkillCategorySeeder> _logger;

    public SkillCategorySeeder(AppDbContext db, ILogger<SkillCategorySeeder> logger)
    {
        _db = db;
        _logger = logger;
    }

    public int Order => 10;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _db.SkillCategories.AnyAsync(cancellationToken))
        {
            _logger.LogInformation("Skill categories already exist. Skipping seed.");
            return;
        }

        var categories = new[]
        {
            CreateCategory(FrontendId, "Frontend", "Frontend development technologies", 1),
            CreateCategory(BackendId, "Backend", "Backend development technologies", 2),
            CreateCategory(DatabaseId, "Database", "Database technologies", 3),
            CreateCategory(DevOpsId, "DevOps", "DevOps and infrastructure", 4),
            CreateCategory(MobileId, "Mobile", "Mobile development", 5),
            CreateCategory(ToolsId, "Tools", "Development tools", 6),
            CreateCategory(SoftSkillsId, "Soft Skills", "Non-technical skills", 7),
            CreateCategory(TestingId, "Testing", "Testing and QA", 8),
            CreateCategory(SecurityId, "Security", "Security and authentication", 9),
            CreateCategory(CloudId, "Cloud", "Cloud platforms and services", 10),
        };

        _db.SkillCategories.AddRange(categories);
        await _db.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Seeded {Count} skill categories.", categories.Length);
    }

    private static SkillCategory CreateCategory(Guid id, string name, string description, int order)
    {
        var category = SkillCategory.Create(name, description, order);
        category.SetId(id);
        return category;
    }
}