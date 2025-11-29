using System;
using System.Threading.Tasks;
using MelodyMatch.Users;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace MelodyMatch.Authentication;

public class BannedUserValidationHandler : IOpenIddictServerHandler<ValidateTokenRequestContext>, ITransientDependency
{
    private readonly IdentityUserManager _identityUserManager;
    private readonly IMelodyMatchUserRepository _melodyMatchUserRepository;
    private readonly IUserBanRepository _userBanRepository;

    public BannedUserValidationHandler(
        IdentityUserManager identityUserManager,
        IMelodyMatchUserRepository melodyMatchUserRepository,
        IUserBanRepository userBanRepository)
    {
        _identityUserManager = identityUserManager;
        _melodyMatchUserRepository = melodyMatchUserRepository;
        _userBanRepository = userBanRepository;
    }

    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<ValidateTokenRequestContext>()
            .UseScopedHandler<BannedUserValidationHandler>()
            .SetOrder(int.MaxValue - 100)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public async ValueTask HandleAsync(ValidateTokenRequestContext context)
    {
        if (context.Request.IsPasswordGrantType())
        {
            var username = context.Request.Username;
            
            if (string.IsNullOrEmpty(username))
            {
                return;
            }

            try
            {
                var identityUser = await _identityUserManager.FindByNameAsync(username) 
                    ?? await _identityUserManager.FindByEmailAsync(username);

                if (identityUser == null)
                {
                    return;
                }

                var melodyMatchUser = await _melodyMatchUserRepository.GetByIdentityUserIdAsync(identityUser.Id);
                
                if (melodyMatchUser == null)
                {
                    return;
                }

                var activeBan = await _userBanRepository.GetActiveBanByUserIdAsync(melodyMatchUser.Id);
                
                if (activeBan != null)
                {
                    var reason = activeBan.Reason ?? "Violation";
                    context.Reject(
                        error: OpenIddictConstants.Errors.AccessDenied,
                        description: $"Your account was banned. Reason: {reason}",
                        uri: null);
                    
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        
        await ValueTask.CompletedTask;
    }
}

