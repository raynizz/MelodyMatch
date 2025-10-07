using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MelodyMatch.Data;
using Volo.Abp.DependencyInjection;

namespace MelodyMatch.EntityFrameworkCore;

public class EntityFrameworkCoreMelodyMatchDbSchemaMigrator
    : IMelodyMatchDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreMelodyMatchDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the MelodyMatchDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<MelodyMatchDbContext>()
            .Database
            .MigrateAsync();
    }
}
