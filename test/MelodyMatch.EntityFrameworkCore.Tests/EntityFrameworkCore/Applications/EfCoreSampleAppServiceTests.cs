using MelodyMatch.Samples;
using Xunit;

namespace MelodyMatch.EntityFrameworkCore.Applications;

[Collection(MelodyMatchTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<MelodyMatchEntityFrameworkCoreTestModule>
{

}
