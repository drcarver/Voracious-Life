namespace Voracious.Gutenberg;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseVoraciousGutenberg(this IServiceCollection collection)
    {
        collection
            .AddDbContext<GutenbergCatalogDataContext>()
            ;

        return collection;
    }
}
