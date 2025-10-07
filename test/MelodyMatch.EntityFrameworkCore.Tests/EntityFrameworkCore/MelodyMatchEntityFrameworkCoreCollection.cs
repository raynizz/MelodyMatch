using Xunit;

namespace MelodyMatch.EntityFrameworkCore;

[CollectionDefinition(MelodyMatchTestConsts.CollectionDefinitionName)]
public class MelodyMatchEntityFrameworkCoreCollection : ICollectionFixture<MelodyMatchEntityFrameworkCoreFixture>
{

}
