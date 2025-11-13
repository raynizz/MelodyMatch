using System;
using System.Threading.Tasks;
using MelodyMatch.ShownUserProfiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace MelodyMatch.Workers;

public class ShownProfilesCleanupWorker : AsyncPeriodicBackgroundWorkerBase, ITransientDependency
{
    private const int ExpiredShownUserDays = 3;
    
    public ShownProfilesCleanupWorker(
        AbpAsyncTimer timer,
        IServiceScopeFactory scopeFactory)
        : base(timer, scopeFactory)
    {
        Timer.Period = 1000 * 60 * 60 * 6; // each 6 hours
    }

    protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext context)
    {
        Logger.LogInformation("Running shown profiles cleanup...");

        using var scope = ServiceScopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IShownUserProfileRepository>();

        var expired = await repository.GetExpiredShownUserProfilesAsync(DateTime.UtcNow.AddDays(-ExpiredShownUserDays));

        if (expired.Count > 0)
        {
            await repository.DeleteManyAsync(expired);
            Logger.LogInformation($"Cleaned up {expired.Count} expired shown profiles.");
        }
        else
        {
            Logger.LogInformation("No expired shown profiles found.");
        }
    }
}
