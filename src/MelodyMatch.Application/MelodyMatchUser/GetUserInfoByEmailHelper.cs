using System.Threading.Tasks;
using MelodyMatch.MelodyMatchUser.DTOs;
using MelodyMatch.Users;
using Volo.Abp.Users;

namespace MelodyMatch.MelodyMatchUser;

internal class GetUserInfoByEmailHelper
{
    private readonly IMelodyMatchUserRepository _melodyMatchUserRepository;

    public GetUserInfoByEmailHelper(IMelodyMatchUserRepository melodyMatchUserRepository)
    {
        _melodyMatchUserRepository = melodyMatchUserRepository;
    }

    public async Task<UserInfoDto?> GetUserInfoByEmail(string email)
    {
        var user = await _melodyMatchUserRepository.GetUserByEmail(email);

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

        return userInfoDto;
    }
}