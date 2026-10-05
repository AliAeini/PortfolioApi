using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioApi.Models;

namespace PortfolioApi.Data.Configurations;

public class ProfileConfiguration : BaseEntityConfiguration<Profile>
{
    public override void Configure(EntityTypeBuilder<Profile> builder)
    {
        base.Configure(builder);

        builder.ToTable("Profiles");

        builder.Property(p => p.FullName).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Bio).IsRequired().HasMaxLength(2000);
        builder.Property(p => p.ShortBio).HasMaxLength(500);
        builder.Property(p => p.JobTitle).HasMaxLength(200);
        builder.Property(p => p.AvatarUrl).HasMaxLength(500);
        builder.Property(p => p.CoverImageUrl).HasMaxLength(500);
        builder.Property(p => p.Email).HasMaxLength(200);
        builder.Property(p => p.PhoneNumber).HasMaxLength(50);
        builder.Property(p => p.Location).HasMaxLength(200);
        builder.Property(p => p.Website).HasMaxLength(500);
        builder.Property(p => p.Nationality).HasMaxLength(100);
        builder.Property(p => p.Languages).HasMaxLength(500);
        builder.Property(p => p.Hobbies).HasMaxLength(1000);

        builder.HasIndex(p => p.Email)
            .IsUnique()
            .HasFilter("\"Email\" IS NOT NULL AND \"Status\" <> 5");

        builder.HasMany(p => p.SocialLinks)
            .WithOne(s => s.Profile)
            .HasForeignKey(s => s.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.JobCategoryId);
    }
}