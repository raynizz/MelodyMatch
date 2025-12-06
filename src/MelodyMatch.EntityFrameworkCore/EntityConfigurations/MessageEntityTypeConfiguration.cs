using MelodyMatch.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MelodyMatch.EntityConfigurations;

public class MessageEntityTypeConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable(MelodyMatchConsts.DbTablePrefix + "Messages", MelodyMatchConsts.DbSchema);

        builder.ConfigureByConvention();
        
        builder.Property(x => x.SenderId).IsRequired();
        builder.HasOne(x => x.Sender)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.SenderId)
            .IsRequired();
        
        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(2048);
        
        builder.Property(x => x.IsRead)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(x => x.ChatId).IsRequired();
        builder.HasOne(x => x.Chat)
            .WithMany(x => x.Messages)
            .HasForeignKey(x => x.ChatId)
            .IsRequired();
    }
}