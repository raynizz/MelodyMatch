using MelodyMatch.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Modularity;

namespace MelodyMatch.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(MelodyMatchEntityFrameworkCoreModule),
    typeof(MelodyMatchApplicationContractsModule)
    )]
public class MelodyMatchDbMigratorModule : AbpModule
{
}
