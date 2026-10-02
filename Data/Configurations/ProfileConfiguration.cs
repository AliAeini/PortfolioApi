using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PortfolioApi.Models;
using Microsoft.EntityFrameworkCore;  

namespace PortfolioApi.Data.Configurations;

public class ProfileConfiguration : BaseEntityConfiguration<Profile>
{
    public override void Configure(EntityTypeBuilder<Profile> builder)
    {
        base.Configure(builder);

        builder.ToTable("Profiles");

        builder.Property(p => p.FullName).IsRequired().HasMaxLength(200);

        builder.Property(p => p.Bio).IsRequired().HasMaxLength(2000);

        builder.Property(p => p.AvatarUrl).HasMaxLength(500);

        builder.Property(p => p.Email).HasMaxLength(200);

        builder.Property(p => p.Location).HasMaxLength(200);

        builder.HasIndex(p => p.Email);
    }
}