using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Volo.Abp.DependencyInjection;

namespace MelodyMatch.Middleware;

public class CultureMiddleware : IMiddleware, ITransientDependency
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var requestCulture = context.Features.Get<IRequestCultureFeature>();
        
        if (requestCulture?.RequestCulture != null)
        {
            var culture = requestCulture.RequestCulture.Culture;
            var uiCulture = requestCulture.RequestCulture.UICulture;
            
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = uiCulture;
        }
        
        await next(context);
    }
}

