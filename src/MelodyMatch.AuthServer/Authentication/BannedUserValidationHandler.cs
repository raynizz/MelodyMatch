using System;
using System.Threading.Tasks;
using MelodyMatch.Localization;
using MelodyMatch.Users;
using Microsoft.Extensions.Localization;
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
    private readonly IStringLocalizer<MelodyMatchResource> _localizer;

    public BannedUserValidationHandler(
        IdentityUserManager identityUserManager,
        IMelodyMatchUserRepository melodyMatchUserRepository,
        IUserBanRepository userBanRepository,
        IStringLocalizer<MelodyMatchResource> localizer)
    {
        _identityUserManager = identityUserManager;
        _melodyMatchUserRepository = melodyMatchUserRepository;
        _userBanRepository = userBanRepository;
        _localizer = localizer;
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
                    var localizedMessage = _localizer[MelodyMatchDomainErrorCodes.MelodyMatchUser.UserProfileWasBanned];
                               
                    var formattedMessage = localizedMessage.Value + activeBan.Reason;
                    
                    context.Reject(
                        error: OpenIddictConstants.Errors.AccessDenied,
                        description: formattedMessage,
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

