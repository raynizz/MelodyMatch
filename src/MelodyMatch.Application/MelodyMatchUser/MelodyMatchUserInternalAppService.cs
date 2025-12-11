using System.Threading.Tasks;
using MelodyMatch.MelodyMatchUser.DTOs;
using MelodyMatch.MelodyMatchUser.Services;
using MelodyMatch.Users;
using Microsoft.Extensions.Configuration;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace MelodyMatch.MelodyMatchUser;

[RemoteService(false)]
public class MelodyMatchUserInternalAppService : ApplicationService, IMelodyMatchUserInternalAppService
{
    private readonly GetUserInfoByEmailHelper _getUserInfoByEmailHelper;
    
    public MelodyMatchUserInternalAppService(
        IConfiguration configuration,
        IMelodyMatchUserRepository repository)
    {
        _getUserInfoByEmailHelper = new GetUserInfoByEmailHelper(repository);
    }

    public async Task<UserInfoDto?> GetUserInfoByEmail(string email)
    {
        return await _getUserInfoByEmailHelper.GetUserInfoByEmail(email);
    }
}