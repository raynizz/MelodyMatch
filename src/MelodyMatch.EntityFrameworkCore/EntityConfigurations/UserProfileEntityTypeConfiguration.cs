using MelodyMatch.UserProfiles;
using MelodyMatch.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MelodyMatch.EntityConfigurations;

public class UserProfileEntityTypeConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable(MelodyMatchConsts.DbTablePrefix + "UserProfiles", MelodyMatchConsts.DbSchema);

        builder.ConfigureByConvention();

        builder.Property(x => x.MelodyMatchUserId).IsRequired();
        builder.HasOne(x => x.MelodyMatchUser)
            .WithOne(x => x.UserProfile)
            .HasForeignKey<UserProfile>(x => x.MelodyMatchUserId)
            .IsRequired();
        builder.HasIndex(x => x.MelodyMatchUserId).IsUnique();
        
        builder.Property(x => x.Age).IsRequired();
        
        builder.Property(x => x.Bio).HasMaxLength(1000);
        
        builder.Property(x => x.Location).HasMaxLength(50);
        
        builder.Property(x => x.PreferredGenders).IsRequired();

        builder.Property(x => x.PreferredMinAge)
            .HasDefaultValue(null)
            .IsRequired(false);
        
        builder.Property(x => x.PreferredMaxAge)
            .HasDefaultValue(null)
            .IsRequired(false);
        
        builder.Property(x => x.Interests)
            .HasDefaultValue(null)
            .IsRequired(false);
    }
}