using MelodyMatch.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace MelodyMatch.Controllers;

public abstract class MelodyMatchController : AbpControllerBase
{
    protected MelodyMatchController()
    {
        LocalizationResource = typeof(MelodyMatchResource);
    }
}
