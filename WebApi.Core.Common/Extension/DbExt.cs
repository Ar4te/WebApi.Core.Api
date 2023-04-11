using Microsoft.Extensions.DependencyInjection;
using WebApi.Core.Common.DB;

namespace WebApi.Core.Common.Extension;

public static class DbExt
{
    public static void AdddBSetup(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));
        services.AddScoped<DBSeed>();
        services.AddScoped<CustomContext>();
    }
}
