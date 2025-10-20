using MelodyMatch.Complaints;
using MelodyMatch.Enums.Complaints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace MelodyMatch.EntityConfigurations;

public class ComplaintEntityTypeConfiguration : IEntityTypeConfiguration<Complaint>
{
    public void Configure(EntityTypeBuilder<Complaint> builder)
    {
        builder.ToTable(MelodyMatchConsts.DbTablePrefix + "Complaints", MelodyMatchConsts.DbSchema);

        builder.ConfigureByConvention();

        builder.Property(x => x.ReporterId).IsRequired();
        builder.HasOne(x => x.Reporter)
            .WithMany(x => x.SentComplaints)
            .HasForeignKey(x => x.ReporterId)
            .IsRequired();
        
        builder.Property(x => x.ReportedUserId).IsRequired();
        builder.HasOne(x => x.ReportedUser)
            .WithMany(x => x.ReceivedComplaints)
            .HasForeignKey(x => x.ReportedUserId)
            .IsRequired();
        
        builder.Property(x => x.Reason)
            .IsRequired()
            .HasMaxLength(2048);
        
        builder.Property(x => x.Status)
            .IsRequired()
            .HasDefaultValue(ComplaintStatus.Sent);
    }
}