using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace MelodyMatch.Data;

/* This is used if database provider does't define
 * IMelodyMatchDbSchemaMigrator implementation.
 */
public class NullMelodyMatchDbSchemaMigrator : IMelodyMatchDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
