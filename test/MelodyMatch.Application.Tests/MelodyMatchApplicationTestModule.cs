using Volo.Abp.Modularity;

namespace MelodyMatch;

[DependsOn(
    typeof(MelodyMatchApplicationModule),
    typeof(MelodyMatchDomainTestModule)
)]
public class MelodyMatchApplicationTestModule : AbpModule
{

}
