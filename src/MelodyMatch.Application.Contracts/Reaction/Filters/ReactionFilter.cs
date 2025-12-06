using System;
using MelodyMatch.Enums.Reactions;
using Volo.Abp.Application.Dtos;

namespace MelodyMatch.Reaction.Filters;

public class ReactionFilter : PagedAndSortedResultRequestDto
{
    public Guid? FromUserId { get; set; } = default;
    
    public Guid? ToUserId { get; set; } = default;
    
    public ReactionType? Type { get; set; } = default;
    
    public string? Message { get; set; } = default;
    
    public ReactionFilter()
    {
    }
}