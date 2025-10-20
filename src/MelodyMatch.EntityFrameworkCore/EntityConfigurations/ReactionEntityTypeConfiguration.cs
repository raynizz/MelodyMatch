using MelodyMatch.Enums.Reactions;
using MelodyMatch.Reactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MelodyMatch.EntityConfigurations;

public class ReactionEntityTypeConfiguration : IEntityTypeConfiguration<Reaction>
{
    public void Configure(EntityTypeBuilder<Reaction> builder)
    {
        builder.ToTable(MelodyMatchConsts.DbTablePrefix + "Reactions", MelodyMatchConsts.DbSchema);

        builder.ConfigureByConvention();
        
        builder.Property(x => x.FromUserId).IsRequired();
        builder.HasOne(x => x.FromUser)
            .WithMany(x => x.SentReactions)
            .HasForeignKey(x => x.FromUserId)
            .IsRequired();
        
        builder.Property(x => x.ToUserId).IsRequired();
        builder.HasOne(x => x.ToUser)
            .WithMany(x => x.ReceivedReactions)
            .HasForeignKey(x => x.ToUserId)
            .IsRequired();
        
        builder.Property(x => x.Type)
            .HasDefaultValue(ReactionType.Skip)
            .IsRequired();
        
        builder.Property(x => x.Message).IsRequired(false);
    }
}