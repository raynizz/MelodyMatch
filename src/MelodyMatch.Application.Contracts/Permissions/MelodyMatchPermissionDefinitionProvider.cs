using MelodyMatch.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace MelodyMatch.Permissions;

public class MelodyMatchPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(MelodyMatchPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(MelodyMatchPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<MelodyMatchResource>(name);
    }
}
