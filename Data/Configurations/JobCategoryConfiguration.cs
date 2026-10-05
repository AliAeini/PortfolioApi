using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioApi.Models;

namespace PortfolioApi.Data.Configurations;

public class JobCategoryConfiguration : BaseEntityConfiguration<JobCategory>
{
    public override void Configure(EntityTypeBuilder<JobCategory> builder)
    {
        base.Configure(builder);

        builder.ToTable("JobCategories");

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Description).HasMaxLength(500);
        builder.Property(c => c.DisplayOrder).HasDefaultValue(0);

        builder.HasIndex(c => c.Name)
            .IsUnique()
            .HasFilter("\"Name\" IS NOT NULL AND \"Status\" <> 5");

        builder.HasMany(c => c.Profiles)
            .WithOne(p => p.JobCategory)
            .HasForeignKey(p => p.JobCategoryId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}