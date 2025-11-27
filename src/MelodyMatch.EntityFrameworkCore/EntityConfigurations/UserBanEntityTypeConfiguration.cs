using MelodyMatch.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MelodyMatch.EntityConfigurations;

public class UserBanEntityTypeConfiguration : IEntityTypeConfiguration<UserBan>
{
    public void Configure(EntityTypeBuilder<UserBan> builder)
    {
        builder.ToTable(MelodyMatchConsts.DbTablePrefix + "UserBans", MelodyMatchConsts.DbSchema);

        builder.ConfigureByConvention();

        builder.Property(x => x.UserId).IsRequired();
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired();
        
        builder.Property(x => x.Reason)
            .IsRequired()
            .HasMaxLength(2048);
        
        builder.Property(x => x.ComplaintId)
            .IsRequired(false);
        
        builder.Property(x => x.ExpiresAt)
            .IsRequired(false);
        
        builder.Property(x => x.IsPermanent)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);
        
        builder.HasIndex(x => new { x.UserId, x.IsActive });
    }
}

