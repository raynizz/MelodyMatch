using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace MelodyMatch.ProfilePhotos;

public interface IProfilePhotoRepository : IRepository<ProfilePhoto, Guid>
{
    Task<List<ProfilePhoto>> GetUnusedPhotosAsync(DateTime olderThan);
    
    Task<List<string>> GetAllUsedFileNamesAsync();
    
    Task<int> CountByUserProfileIdAsync(Guid userProfileId);
    
    Task<HashSet<string>> GetAllProfilePhotoFileNamesAsync();
}