using Microsoft.Extensions.DependencyInjection;

namespace Voracious.Database;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseVoraciousDatabase(this IServiceCollection collection)
    {
        collection
            .AddDbContext<CatalogDataContext>()
            ;

        return collection;
    }
}
