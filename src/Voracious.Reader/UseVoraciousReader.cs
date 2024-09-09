using Microsoft.Extensions.DependencyInjection;

namespace Voracious.Database;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseVoraciousReader(this IServiceCollection collection)
    {
        //collection
            //.AddSingleton<IUnhandeledException, UnhandeledException>
            ;

        return collection;
    }
}
