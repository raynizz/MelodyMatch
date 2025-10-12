using MelodyMatch.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MelodyMatch.EntityConfigurations;

public class MelodyMatchUserEntityTypeConfiguration : IEntityTypeConfiguration<MelodyMatchUser>
{
    public void Configure(EntityTypeBuilder<MelodyMatchUser> builder)
    {
        builder.ToTable(MelodyMatchConsts.DbTablePrefix + "MelodyMatchUsers", MelodyMatchConsts.DbSchema);

        builder.ConfigureByConvention();

        builder.Property(x => x.IdentityUserId).IsRequired();
        builder.HasOne(x => x.IdentityUser)
            .WithOne()
            .HasForeignKey<MelodyMatchUser>(x => x.IdentityUserId)
            .IsRequired();
        builder.HasIndex(x => x.IdentityUserId).IsUnique();
        
        builder.Property(x => x.Gender).IsRequired();
    }
}