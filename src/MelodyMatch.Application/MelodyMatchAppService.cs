using System;
using System.Collections.Generic;
using System.Text;
using MelodyMatch.Localization;
using Volo.Abp.Application.Services;

namespace MelodyMatch;

/* Inherit your application services from this class.
 */
public abstract class MelodyMatchAppService : ApplicationService
{
    protected MelodyMatchAppService()
    {
        LocalizationResource = typeof(MelodyMatchResource);
    }
}
