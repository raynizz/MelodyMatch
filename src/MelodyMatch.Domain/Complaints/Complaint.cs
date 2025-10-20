using System;
using MelodyMatch.Enums.Complaints;
using MelodyMatch.Users;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace MelodyMatch.Complaints;

public class Complaint : FullAuditedAggregateRoot<Guid>
{
    public Guid ReporterId { get; set; }
    
    public MelodyMatchUser Reporter { get; set; }
    
    public Guid ReportedUserId { get; set; }
    
    public MelodyMatchUser ReportedUser { get; set; }
    
    public string Reason { get; set; } // TODO: make enum

    public ComplaintStatus Status { get; set; } = ComplaintStatus.Sent;

    public Complaint()
    {
    }
    
    public Complaint(Guid reporterId, Guid reportedUserId, string reason, ComplaintStatus status)
    {
        Id = SimpleGuidGenerator.Instance.Create();
        ReporterId = reporterId;
        ReportedUserId = reportedUserId;
        Reason = reason;
        Status = status;
    }
    
    public Complaint(Guid id, Guid reporterId, Guid reportedUserId, string reason, ComplaintStatus status) : base(id)
    {
        ReporterId = reporterId;
        ReportedUserId = reportedUserId;
        Reason = reason;
        Status = status;
    }
}