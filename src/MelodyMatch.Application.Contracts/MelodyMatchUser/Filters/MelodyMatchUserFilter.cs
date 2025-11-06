using MelodyMatch.Enums.MelodyMatchUser;
using Volo.Abp.Application.Dtos;

namespace MelodyMatch.MelodyMatchUser.Filters;

public class MelodyMatchUserFilter : PagedAndSortedResultRequestDto
{
    public GenderType? Gender { get; set; }
    
    // TODO: add more filter properties if needed
    
    public MelodyMatchUserFilter()
    {
    }
}