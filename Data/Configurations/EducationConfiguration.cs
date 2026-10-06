using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioApi.Models;

namespace PortfolioApi.Data.Configurations;

public class EducationConfiguration : BaseEntityConfiguration<Education>
{
    public override void Configure(EntityTypeBuilder<Education> builder)
    {
        base.Configure(builder);

        builder.ToTable("Educations");

        builder.Property(e => e.Institution).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Degree)
            .HasConversion<int>()
            .IsRequired();
        builder.Property(e => e.Field).IsRequired().HasMaxLength(200);
        builder.Property(e => e.Description).HasMaxLength(2000);
        builder.Property(e => e.Location).HasMaxLength(200);
        builder.Property(e => e.Grade).HasMaxLength(50);
        builder.Property(e => e.DisplayOrder).HasDefaultValue(0);

        builder.HasOne(e => e.Profile)
            .WithMany(p => p.Educations)
            .HasForeignKey(e => e.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(e => e.ProfileId);
        builder.HasIndex(e => e.StartDate);
    }
}