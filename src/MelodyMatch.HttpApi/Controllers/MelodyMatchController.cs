using MelodyMatch.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace MelodyMatch.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class MelodyMatchController : AbpControllerBase
{
    protected MelodyMatchController()
    {
        LocalizationResource = typeof(MelodyMatchResource);
    }
}
