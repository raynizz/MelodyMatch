using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;

namespace MelodyMatch.Users;

public class UserBan : FullAuditedAggregateRoot<Guid>
{
    public Guid UserId { get; set; }
    
    public MelodyMatchUser User { get; set; }
    
    public string Reason { get; set; }
    
    public Guid? ComplaintId { get; set; }
    
    public DateTime? ExpiresAt { get; set; }
    
    public bool IsPermanent { get; set; }
    
    public bool IsActive { get; set; } = true;

    protected UserBan()
    {
    }
    
    public UserBan(Guid userId, string reason, Guid? complaintId = null, DateTime? expiresAt = null, bool isPermanent = false)
    {
        Id = SimpleGuidGenerator.Instance.Create();
        UserId = userId;
        Reason = reason;
        ComplaintId = complaintId;
        ExpiresAt = expiresAt;
        IsPermanent = isPermanent;
        IsActive = true;
    }
}

