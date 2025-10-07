using Volo.Abp.Modularity;

namespace MelodyMatch;

public abstract class MelodyMatchApplicationTestBase<TStartupModule> : MelodyMatchTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
