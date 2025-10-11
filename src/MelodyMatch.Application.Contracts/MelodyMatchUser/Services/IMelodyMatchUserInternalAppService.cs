using System.Threading.Tasks;
using MelodyMatch.MelodyMatchUser.DTOs;
using Volo.Abp.Application.Services;

namespace MelodyMatch.MelodyMatchUser.Services;

public interface IMelodyMatchUserInternalAppService : IApplicationService
{
    Task<UserInfoDto?> GetUserInfoByEmail(string email);
}