using MelodyMatch.Chats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MelodyMatch.EntityConfigurations;

public class ChatEntityTypeConfiguration : IEntityTypeConfiguration<Chat>
{
    public void Configure(EntityTypeBuilder<Chat> builder)
    {
        builder.ToTable(MelodyMatchConsts.DbTablePrefix + "Chats", MelodyMatchConsts.DbSchema);

        builder.ConfigureByConvention();
        
        builder.HasMany(x => x.Participants)
            .WithOne(x => x.Chat)
            .HasForeignKey(x => x.ChatId)
            .IsRequired();
        
    }
}