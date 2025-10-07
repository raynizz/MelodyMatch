using Volo.Abp.Modularity;

namespace MelodyMatch;

/* Inherit from this class for your domain layer tests. */
public abstract class MelodyMatchDomainTestBase<TStartupModule> : MelodyMatchTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
