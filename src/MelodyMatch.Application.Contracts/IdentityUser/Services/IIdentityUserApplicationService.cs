using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace MelodyMatch.IdentityUser.Services;

public interface IIdentityUserApplicationService : IApplicationService
{
    Task<IdentityUserDto> GetByIdAsync(Guid id);
    
    Task<IdentityUserDto> GetByUsernameAsync(string username);
    
    Task<IdentityUserDto> GetByEmailAsync(string email);
    
    Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto request);
    
    Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto request);
    
    Task<IdentityUserDto> DeleteAsync(Guid id);
}