using System.Threading.Tasks;
using MelodyMatch.MelodyMatchUser.DTOs;
using MelodyMatch.MelodyMatchUser.Services;
using MelodyMatch.Repositories;
using Microsoft.Extensions.Configuration;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace MelodyMatch.MelodyMatchUser;

public class MelodyMatchUserInternalAppService : ApplicationService, IMelodyMatchUserInternalAppService
{
    private readonly IUserRepository _repository;
    private readonly GetUserInfoByEmailHelper _getUserInfoByEmailHelper;
    
    public MelodyMatchUserInternalAppService(
        IConfiguration configuration,
        IUserRepository repository)
    {
        _repository = repository;
        _getUserInfoByEmailHelper = new GetUserInfoByEmailHelper(_repository);
    }

    public async Task<UserInfoDto?> GetUserInfoByEmail(string email)
    {
        return await _getUserInfoByEmailHelper.GetUserInfoByEmail(email);
    }
}