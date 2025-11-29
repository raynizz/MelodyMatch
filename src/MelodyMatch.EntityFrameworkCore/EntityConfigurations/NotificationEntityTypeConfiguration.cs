using MelodyMatch.Enums.Notifications;
using MelodyMatch.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MelodyMatch.EntityConfigurations;

public class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable(MelodyMatchConsts.DbTablePrefix + "Notifications", MelodyMatchConsts.DbSchema);

        builder.ConfigureByConvention();

        builder.Property(x => x.UserId).IsRequired();
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired();
        
        builder.Property(x => x.Type)
            .IsRequired()
            .HasDefaultValue(NotificationType.General);
        
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(x => x.Message)
            .IsRequired()
            .HasMaxLength(2048);
        
        builder.Property(x => x.IsRead)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(x => x.RelatedEntityId)
            .IsRequired(false);
        
        builder.HasIndex(x => new { x.UserId, x.IsRead });
    }
}

