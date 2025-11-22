using System;
using System.Threading.Tasks;
using MelodyMatch.Constants;
using MelodyMatch.IdentityUser.Services;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;

namespace MelodyMatch.IdentityUser;

[Authorize(Roles = RolesConsts.Admin + "," + RolesConsts.Dater)]
public class IdentityUserApplicationService : ApplicationService, IIdentityUserApplicationService
{
    private readonly IIdentityUserRepository _identityUserRepository;
    
    public IdentityUserApplicationService(IIdentityUserRepository identityUserRepository)
    {
        _identityUserRepository = identityUserRepository;
    }
    
    public async Task<IdentityUserDto> GetByIdAsync(Guid id)
    {
        var identityUser = await _identityUserRepository.FindAsync(id);
        
        return ObjectMapper.Map<Volo.Abp.Identity.IdentityUser, IdentityUserDto>(identityUser);
    }

    public async Task<IdentityUserDto> GetByUsernameAsync(string username)
    {
        var usernameNormalized = username.ToUpper();
        var identityUser = await _identityUserRepository.FindByNormalizedUserNameAsync(usernameNormalized);
        
        return ObjectMapper.Map<Volo.Abp.Identity.IdentityUser, IdentityUserDto>(identityUser);
    }

    public async Task<IdentityUserDto> GetByEmailAsync(string email)
    {
        var emailNormalized = email.ToUpper();
        var identityUser = await _identityUserRepository.FindByNormalizedEmailAsync(email);
        
        return ObjectMapper.Map<Volo.Abp.Identity.IdentityUser, IdentityUserDto>(identityUser);
    }

    public async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto request)
    {
        var identityUser = ObjectMapper.Map<IdentityUserCreateDto, Volo.Abp.Identity.IdentityUser>(request);
        
        var createdIdentityUser = await _identityUserRepository.InsertAsync(identityUser, autoSave: true);
        
        return ObjectMapper.Map<Volo.Abp.Identity.IdentityUser, IdentityUserDto>(createdIdentityUser);
    }

    public async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto request)
    {
        var identityUser = await _identityUserRepository.GetAsync(id);
        ObjectMapper.Map(request, identityUser);
        var updatedIdentityUser = await _identityUserRepository.UpdateAsync(identityUser, autoSave: true);
        
        return ObjectMapper.Map<Volo.Abp.Identity.IdentityUser, IdentityUserDto>(updatedIdentityUser);
    }

    public async Task<IdentityUserDto> DeleteAsync(Guid id)
    {
        var identityUser = await _identityUserRepository.GetAsync(id);
        await _identityUserRepository.DeleteAsync(identityUser);
        
        return ObjectMapper.Map<Volo.Abp.Identity.IdentityUser, IdentityUserDto>(identityUser);
    }
}