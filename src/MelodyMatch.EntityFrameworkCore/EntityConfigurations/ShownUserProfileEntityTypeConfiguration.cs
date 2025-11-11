using MelodyMatch.ShownUserProfiles;
using MelodyMatch.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MelodyMatch.EntityConfigurations;

public class ShownUserProfileEntityTypeConfiguration : IEntityTypeConfiguration<ShownUserProfile>
{
    public void Configure(EntityTypeBuilder<ShownUserProfile> builder)
    {
        builder.ToTable(MelodyMatchConsts.DbTablePrefix + "ShownUserProfiles", MelodyMatchConsts.DbSchema);

        builder.ConfigureByConvention();

        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.ShownUserId).IsRequired();
        builder.Property(x => x.Reacted).HasDefaultValue(false);

        builder.HasIndex(x => new { x.UserId, x.ShownUserId }).IsUnique();

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ShownUser)
            .WithMany()
            .HasForeignKey(x => x.ShownUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}