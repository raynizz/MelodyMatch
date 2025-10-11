using MelodyMatch.Entities;
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

        builder.Property(x => x.UserName).IsRequired();
        builder.HasIndex(x => x.UserName).IsUnique();
        
        builder.Property(x => x.FirstName).IsRequired();
        
        builder.Property(x => x.Gender).IsRequired();
        
        builder.Property(x => x.Email).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
    }
}