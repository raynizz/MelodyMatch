using System;
using System.Threading.Tasks;
using MelodyMatch.Users;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace MelodyMatch.Contexts.MelodyMatchUser;

public class CurrentMelodyMatchUser : ICurrentMelodyMatchUser, ITransientDependency
{
    private readonly ICurrentUser _currentUser;
    private readonly IMelodyMatchUserRepository _melodyMatchUserRepository;
    
    public CurrentMelodyMatchUser(
        ICurrentUser currentUser,
        IMelodyMatchUserRepository melodyMatchUserRepository)
    {
        _currentUser = currentUser;
        _melodyMatchUserRepository = melodyMatchUserRepository;
    }
    
    public async Task<Users.MelodyMatchUser> GetAsync()
    {
        var identityId = _currentUser.Id;
        if (identityId == null)
        {
            return null;
        }

        return await _melodyMatchUserRepository.GetByIdentityUserIdAsync(identityId.Value);
        
    }

    public async Task<Guid> GetIdAsync()
    {
        var user = await GetAsync();
        
        return user.Id;
    }
}