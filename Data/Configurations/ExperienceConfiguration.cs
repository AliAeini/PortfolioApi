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
        builder.Property(e => e.EmploymentType).HasConversion<int>().IsRequired();
        builder.Property(e => e.Description).HasMaxLength(3000);
        builder.Property(e => e.Location).HasMaxLength(200);
        builder.Property(e => e.CompanyUrl).HasMaxLength(500);
        builder.Property(e => e.DisplayOrder).HasDefaultValue(0);

        builder.HasOne(e => e.Profile)
            .WithMany(p => p.Experiences)
            .HasForeignKey(e => e.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.ProfileId);
        builder.HasIndex(e => e.StartDate);
    }
}