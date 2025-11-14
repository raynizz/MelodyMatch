using MelodyMatch.ProfilePhotos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MelodyMatch.EntityConfigurations;

public class ProfilePhotoEntityTypeConfiguration : IEntityTypeConfiguration<ProfilePhoto>
{
    public void Configure(EntityTypeBuilder<ProfilePhoto> builder)
    {
        builder.ToTable(MelodyMatchConsts.DbTablePrefix + "ProfilePhotos", MelodyMatchConsts.DbSchema);
        builder.ConfigureByConvention();

        builder.Property(x => x.UserProfileId)
            .IsRequired();

        builder.Property(x => x.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.IsConfirmed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(x => x.UserProfileId);
        builder.HasIndex(x => x.FileName).IsUnique(false);

        builder.HasOne(x => x.UserProfile)
            .WithMany(x => x.ProfilePhotos)
            .HasForeignKey(x => x.UserProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}