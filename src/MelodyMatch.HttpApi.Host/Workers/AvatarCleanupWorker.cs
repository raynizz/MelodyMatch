using System;
using System.IO;
using System.Threading.Tasks;
using MelodyMatch.Constants;
using MelodyMatch.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace MelodyMatch.File;

public class AvatarCleanupWorker : AsyncPeriodicBackgroundWorkerBase, ITransientDependency
{
    private readonly IWebHostEnvironment _env;

    public AvatarCleanupWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory scopeFactory,
        IWebHostEnvironment env)
        : base(timer, scopeFactory)
    {
        _env = env;
        Timer.Period = 1000 * 60 * 60 * 6; // кожні 6 годин
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext context)
    {
        Logger.LogInformation("Running avatar cleanup...");

        using var scope = ServiceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IMelodyMatchUserRepository>();

        var uploadFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", FileConsts.Avatar.AvatarFolderPath);
        if (!Directory.Exists(uploadFolder))
        {
            Logger.LogWarning("Avatar folder not found: {Path}", uploadFolder);
            return;
        }

        var allFiles = Directory.GetFiles(uploadFolder);
        var usedAvatarUrls = await repository.GetAllAvatarUrlsHashAsync();

        int deleted = 0;
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
                if (age > TimeSpan.FromHours(FileConsts.Avatar.AutoDeleteUnsavedAvatarsTimeHours))
                {
                    System.IO.File.Delete(filePath);
                    deleted++;
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Error deleting avatar file: {File}", filePath);
            }
        }

        Logger.LogInformation("Avatar cleanup finished. Deleted {Count} unused avatars", deleted);
    }
}