using System;
using System.IO;
using System.Threading.Tasks;
using MelodyMatch.Constants;
using MelodyMatch.ProfilePhotos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace MelodyMatch.Workers;

public class ProfilePhotoCleanupWorker : AsyncPeriodicBackgroundWorkerBase, ITransientDependency
{
    private readonly IWebHostEnvironment _env;

    public ProfilePhotoCleanupWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory scopeFactory,
        IWebHostEnvironment env)
        : base(timer, scopeFactory)
    {
        _env = env;
        Timer.Period = 1000 * 60 * 60 * 6;
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext context)
    {
        Logger.LogInformation("Running profile photo cleanup...");

        using var scope = ServiceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IProfilePhotoRepository>();

        var uploadFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", FileConsts.Profile.ProfileFolderPath);
        if (!Directory.Exists(uploadFolder))
        {
            Logger.LogWarning("Profile photo folder not found: {Path}", uploadFolder);
            return;
        }

        var allFiles = Directory.GetFiles(uploadFolder);
        var usedPhotoFiles = await repository.GetAllProfilePhotoFileNamesAsync();

        var deleted = 0;
        foreach (var filePath in allFiles)
        {
            try
            {
                var fileName = Path.GetFileName(filePath);

                if (usedPhotoFiles.Contains(fileName))
                {
                    continue;
                }

                var age = DateTime.UtcNow - System.IO.File.GetCreationTimeUtc(filePath);
                if (age > TimeSpan.FromHours(FileConsts.Profile.AutoDeleteUnusedPhotosTimeHours))
                {
                    System.IO.File.Delete(filePath);
                    deleted++;
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Error deleting profile photo file: {File}", filePath);
            }
        }

        Logger.LogInformation("Profile photo cleanup finished. Deleted {Count} unused photos", deleted);
    }
}
