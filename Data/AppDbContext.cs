using Microsoft.EntityFrameworkCore;
using PortfolioApi.Common;
using PortfolioApi.Interfaces;
using PortfolioApi.Models;

namespace PortfolioApi.Data;

public class AppDbContext : DbContext
{
    private readonly ICurrentUserService _currentUser;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        ICurrentUserService currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }

    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<SkillCategory> SkillCategories => Set<SkillCategory>();
    public DbSet<Education> Educations => Set<Education>();
    public DbSet<Experience> Experiences => Set<Experience>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectCategory> ProjectCategories => Set<ProjectCategory>();
    public DbSet<SocialLink> SocialLinks => Set<SocialLink>();
    public DbSet<ProfileSkill> ProfileSkills => Set<ProfileSkill>();
    public DbSet<JobCategory> JobCategories => Set<JobCategory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var currentUserId = _currentUser.UserId;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity.CreatedByUserId is null && currentUserId.HasValue)
                    {
                        entry.Entity.SetCreatedBy(currentUserId);
                    }
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdateTimestamp();
                    entry.Entity.IncreaseRevision();
                    break;

                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.SetStatus(EntityStatus.Deleted);
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}