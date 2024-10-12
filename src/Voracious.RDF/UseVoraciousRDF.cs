using Microsoft.Extensions.DependencyInjection;

using Voracious.Core.Interface;
using Voracious.RDF.Model;

namespace Voracious.Core;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseVoraciousRDF(this IServiceCollection collection)
    {
        collection
            .AddTransient<IBookNavigationCore, BookNavigation>()
            .AddTransient<IResourceCore, Resource>()
            .AddTransient<IFileFormatCore, FileFormat>()
            .AddTransient<ICreatorCore, Creator>()
            //.AddTransient<IUserNote, UserNote>()
            //.AddTransient<IUserReview, UserReview>()
             ;
        return collection;
    }
}
