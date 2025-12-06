using System;
using System.Collections.Generic;
using MelodyMatch.Chats;
using MelodyMatch.Complaints;
using MelodyMatch.Enums.MelodyMatchUser;
using MelodyMatch.Reactions;
using MelodyMatch.UserProfiles;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace MelodyMatch.Users;

public class MelodyMatchUser : FullAuditedAggregateRoot<Guid>
{
    public Guid IdentityUserId { get; set; }
    
    public IdentityUser IdentityUser { get; set; }

    public GenderType Gender { get; set; } = GenderType.NotSpecified;
    
    public string AvatarUrl { get; set; } = string.Empty;
    
    public UserProfile UserProfile { get; set; } = default!;
    
    public List<Reaction> SentReactions { get; set; } = new List<Reaction>();
    
    public List<Reaction> ReceivedReactions { get; set; } = new List<Reaction>();
    
    public List<Complaint> SentComplaints { get; set; } = new List<Complaint>();
    
    public List<Complaint> ReceivedComplaints { get; set; } = new List<Complaint>();
    
    public List<Message> Messages { get; set; } = new List<Message>();
    
    public List<ChatParticipant> ChatParticipants { get; set; } = new List<ChatParticipant>();


    // TODO: add roles, spotify, music preferences, etc.

    protected MelodyMatchUser()
    {
    }

    public MelodyMatchUser(Guid identityUserId, GenderType gender)
    {
        Id = SimpleGuidGenerator.Instance.Create();
        IdentityUserId = identityUserId;
        Gender = gender;
    }

    public MelodyMatchUser(Guid id, Guid identityUserId, GenderType gender) : base(id)
    {
        IdentityUserId = identityUserId;
        Gender = gender;
    }
}