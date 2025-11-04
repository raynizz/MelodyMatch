using System.Collections.Generic;
using MelodyMatch.Enums.MelodyMatchUser;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;

namespace MelodyMatch.UserProfile.Filters;

public class UserProfileFilter : PagedAndSortedResultRequestDto
{
    public int? Age { get; set;  } = default;
    
    public string? Bio { get; set;  } = default;
    
    public string? Location { get; set;  } = default;
    
    public List<GenderType>? PreferredGenders { get; set;  } = default;
    
    public int? PreferredMinAge { get; set;  } = default;
    
    public int? PreferredMaxAge { get; set;  } = default;
    
    public UserProfileFilter()
    {
    }
}