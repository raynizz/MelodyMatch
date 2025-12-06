using Microsoft.Extensions.Localization;
using MelodyMatch.Localization;
using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace MelodyMatch;

[Dependency(ReplaceServices = true)]
public class MelodyMatchBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<MelodyMatchResource> _localizer;

    public MelodyMatchBrandingProvider(IStringLocalizer<MelodyMatchResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["MelodyMatch"];
}
