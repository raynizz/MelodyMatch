using System;
using System.Threading.Tasks;
using MelodyMatch.Enums.MelodyMatchUser;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.Identity;

namespace MelodyMatch.Users;

public class IdentityUserCreatedHandler : ILocalEventHandler<EntityCreatedEventData<IdentityUser>>
{
    private readonly IRepository<MelodyMatchUser, Guid> _melodyUserRepository;

    public IdentityUserCreatedHandler(IRepository<MelodyMatchUser, Guid> melodyUserRepository)
    {
        _melodyUserRepository = melodyUserRepository;
    }

    public async Task HandleEventAsync(EntityCreatedEventData<IdentityUser> eventData)
    {
        var melodyUser = new MelodyMatchUser(eventData.Entity.Id, GenderType.NotSpecified);
        
        await _melodyUserRepository.InsertAsync(melodyUser, true);
    }
}