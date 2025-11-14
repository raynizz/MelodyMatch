using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.EntityFrameworkCore;
using MelodyMatch.ProfilePhotos;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace MelodyMatch.Repositories;

public class EfCoreProfilePhotoRepository : EfCoreRepository<MelodyMatchDbContext, ProfilePhoto, Guid>, IProfilePhotoRepository
{
    public EfCoreProfilePhotoRepository(IDbContextProvider<MelodyMatchDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<ProfilePhoto>> GetUnusedPhotosAsync(DateTime olderThan)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.ProfilePhotos
            .Where(x => !x.IsConfirmed && x.CreationTime < olderThan)
            .ToListAsync();
    }

    public async Task<List<string>> GetAllUsedFileNamesAsync()
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.ProfilePhotos
            .Where(x => x.IsConfirmed && !string.IsNullOrEmpty(x.FileName))
            .Select(x => x.FileName)
            .ToListAsync();
    }

    public async Task<int> CountByUserProfileIdAsync(Guid userProfileId)
    {
        var dbContext = await GetDbContextAsync();
        
        return await dbContext.ProfilePhotos.CountAsync(x => x.UserProfileId == userProfileId && x.IsConfirmed);
    }
    
    public async Task<HashSet<string>> GetAllProfilePhotoFileNamesAsync()
    {
        var dbContext = await GetDbContextAsync();
        return await dbContext.ProfilePhotos
            .Where(p => !string.IsNullOrWhiteSpace(p.Url))
            .Select(p => Path.GetFileName(p.Url))
            .ToHashSetAsync(StringComparer.OrdinalIgnoreCase);
    }
}