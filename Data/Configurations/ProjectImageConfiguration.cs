using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioApi.Models;

namespace PortfolioApi.Data.Configurations;

public class ProjectImageConfiguration : BaseEntityConfiguration<ProjectImage>
{
    public override void Configure(EntityTypeBuilder<ProjectImage> builder)
    {
        base.Configure(builder);

        builder.ToTable("ProjectImages");

        builder.Ignore(pi => pi.RowVersion);

        builder.Property(pi => pi.ImageUrl).IsRequired().HasMaxLength(500);
        builder.Property(pi => pi.Caption).HasMaxLength(500);
        builder.Property(pi => pi.IsCover).HasDefaultValue(false);
        builder.Property(pi => pi.DisplayOrder).HasDefaultValue(0);

        builder.HasOne(pi => pi.Project)
            .WithMany(p => p.Images)
            .HasForeignKey(pi => pi.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pi => pi.ProjectId);
        builder.HasIndex(pi => pi.IsCover);
    }
}