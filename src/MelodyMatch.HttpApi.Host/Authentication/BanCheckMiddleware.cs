using System;
using System.Security.Claims;
using System.Threading.Tasks;
using MelodyMatch.Exceptions;
using MelodyMatch.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace MelodyMatch.Authentication;

public class BanCheckMiddleware : IMiddleware, ITransientDependency
{
    private readonly IUserBanRepository _userBanRepository;
    private readonly IMelodyMatchUserRepository _melodyMatchUserRepository;
    
    public BanCheckMiddleware(
        IUserBanRepository userBanRepository,
        IMelodyMatchUserRepository melodyMatchUserRepository)
    {
        _userBanRepository = userBanRepository;
        _melodyMatchUserRepository = melodyMatchUserRepository;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var identityUserIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (Guid.TryParse(identityUserIdClaim, out var identityUserId))
            {
                try
                {
                    var melodyMatchUser = await _melodyMatchUserRepository.GetByIdentityUserIdAsync(identityUserId);
                    
                    if (melodyMatchUser != null)
                    {
                        var ban = await _userBanRepository.GetActiveBanByUserIdAsync(melodyMatchUser.Id);
                        
                        if (ban != null)
                        {
                            await context.SignOutAsync();
                            
                            throw new AccountBannedException(MelodyMatchDomainErrorCodes.MelodyMatchUser.UserProfileWasBanned)
                                .WithData("reason", ban.Reason);
                        }
                    }
                }
                catch (UserFriendlyException)
                {
                    throw;
                }
                catch (Exception)
                {
                }
            }
        }
        
        await next(context);
    }
}