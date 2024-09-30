using Microsoft.Extensions.DependencyInjection;

using Voracious.Database.Interface;

namespace Voracious.Database;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseVoraciousDatabase(this IServiceCollection collection)
    {
        collection
            .AddDbContext<CatalogDataContext>()
            .AddTransient<ICardCatalog, CardCatalog>()
            ;

        return collection;
    }
}
