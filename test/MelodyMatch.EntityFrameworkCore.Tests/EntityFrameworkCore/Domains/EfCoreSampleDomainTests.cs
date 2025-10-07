using MelodyMatch.Samples;
using Xunit;

namespace MelodyMatch.EntityFrameworkCore.Domains;

[Collection(MelodyMatchTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<MelodyMatchEntityFrameworkCoreTestModule>
{

}
