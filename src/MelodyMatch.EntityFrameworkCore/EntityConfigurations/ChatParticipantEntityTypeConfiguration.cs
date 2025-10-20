using MelodyMatch.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MelodyMatch.EntityConfigurations;

public class ChatParticipantEntityTypeConfiguration : IEntityTypeConfiguration<ChatParticipant>
{
    public void Configure(EntityTypeBuilder<ChatParticipant> builder)
    {
        builder.ToTable(MelodyMatchConsts.DbTablePrefix + "ChatParticipants", MelodyMatchConsts.DbSchema);
        
        builder.ConfigureByConvention();

        builder.HasKey(x => new { x.ChatId, x.UserId });

        builder.HasOne(x => x.User)
            .WithMany(x => x.ChatParticipants)
            .HasForeignKey(x => x.UserId)
            .IsRequired();
    }
}