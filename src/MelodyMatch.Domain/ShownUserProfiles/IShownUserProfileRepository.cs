using System;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.ShownUserProfiles;

public interface IShownUserProfileRepository : IRepository<ShownUserProfile, Guid>
{
}