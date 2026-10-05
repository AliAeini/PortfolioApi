using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioApi.Models;

namespace PortfolioApi.Data.Configurations;

public class ExperienceConfiguration : BaseEntityConfiguration<Experience>
{
    public override void Configure(EntityTypeBuilder<Experience> builder)
    {
        base.Configure(builder);

        builder.ToTable("Experiences");

        builder.Property(e => e.Company).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Position).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Description).HasMaxLength(3000);
        builder.Property(e => e.Location).HasMaxLength(200);
        builder.Property(e => e.CompanyUrl).HasMaxLength(500);
        builder.Property(e => e.EmploymentType).HasMaxLength(50);
        builder.Property(e => e.DisplayOrder).HasDefaultValue(0);

        builder.HasIndex(e => e.StartDate);
    }
}