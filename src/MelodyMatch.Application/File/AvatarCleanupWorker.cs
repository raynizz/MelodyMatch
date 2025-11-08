using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MelodyMatch.Constants;
using MelodyMatch.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace MelodyMatch.File;

public class AvatarCleanupWorker : AsyncPeriodicBackgroundWorkerBase
{
    private readonly IWebHostEnvironment _env;
    private readonly IMelodyMatchUserRepository _melodyMatchUserRepository;

    public AvatarCleanupWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory scopeFactory,
        IWebHostEnvironment env,
        IMelodyMatchUserRepository melodyMatchUserRepository)
        : base(timer, scopeFactory)
    {
        _env = env;
        _melodyMatchUserRepository = melodyMatchUserRepository;
        Timer.Period = FileConsts.Avatar.AutoDeleteUnsavedAvatarsTimeHours;
    }
    
    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
    {
        Logger.LogInformation("Running avatar cleanup...");

        var uploadFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", FileConsts.Avatar.AvatarFolderPath);
        if (!Directory.Exists(uploadFolder))
        {
            return;
        }

        var allFiles = Directory.GetFiles(uploadFolder);
        var usedAvatarUrls = await _melodyMatchUserRepository.GetAllAvatarUrlsHashAsync();

        var deleted = 0;
        foreach (var filePath in allFiles)
        {
            try
            {
                var fileName = Path.GetFileName(filePath);

                if (usedAvatarUrls.Contains(fileName))
                {
                    continue;
                }

                var age = DateTime.UtcNow - System.IO.File.GetCreationTimeUtc(filePath);
                if (age > TimeSpan.FromHours(FileConsts.Avatar.MaxFileSizeInBytes))
                {
                    System.IO.File.Delete(filePath);
                    deleted++;
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Error deleting file {Path}", filePath);
            }
        }

        Logger.LogInformation("Avatar cleanup finished. Deleted {Count} unused avatars", deleted);
    }
}