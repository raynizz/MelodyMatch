using Volo.Abp.Settings;

namespace MelodyMatch.Settings;

public class MelodyMatchSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(MelodyMatchSettings.MySetting1));
    }
}
