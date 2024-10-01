using Microsoft.Extensions.DependencyInjection;

using Voracious.EPub;
using Voracious.EPub.Interface;

namespace Voracious.Database;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseVoraciousEPub(this IServiceCollection collection)
    {
        collection
            .AddTransient<IEpubReader, EpubReader>()
            ;

        return collection;
    }
}
