using System.Threading.Tasks;
using MelodyMatch.MelodyMatchUser.DTOs;
using MelodyMatch.Users;
using Volo.Abp.Users;

namespace MelodyMatch.MelodyMatchUser;

internal class GetUserInfoByEmailHelper
{
    private readonly IUserRepository _userRepository;

    public GetUserInfoByEmailHelper(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserInfoDto?> GetUserInfoByEmail(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);

        if (user == null)
        {
            return null;
        }
        var userInfoDto = new UserInfoDto
        {
            FullName = user.IdentityUser.Name + user.IdentityUser.Surname,
            Email = user.IdentityUser.Email,
            Id = user.Id,
            Username = user.IdentityUser.UserName,
            IdentityUserId = user.IdentityUserId
        };
        
        /*
            Roles = user.GenesisUserRoles
                .Select(x => x.GenesisRoleId.ToString())
                .ToList(),
            RoleNames = user.GenesisUserRoles
                .Where(x => x.GenesisRole != null)
                .Select(x => x.GenesisRole.Name)
                .ToList()
         */

        return userInfoDto;
    }
}