using Volo.Abp.Modularity;

namespace MelodyMatch;

[DependsOn(
    typeof(MelodyMatchDomainModule),
    typeof(MelodyMatchTestBaseModule)
)]
public class MelodyMatchDomainTestModule : AbpModule
{

}
