using Microsoft.Extensions.DependencyInjection;

using Voracious.RDF.Interface;
using Voracious.RDF.Model;

namespace Voracious.Core;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseVoraciousRDF(this IServiceCollection collection)
    {
        collection
            .AddTransient<IBookNavigation, BookNavigation>()
            .AddTransient<IResource, Resource>()
            .AddTransient<IFileFormat, FileFormat>()
            .AddTransient<ICreator, Creator>()
            //.AddTransient<IUserNote, UserNote>()
            //.AddTransient<IUserReview, UserReview>()
             ;
        return collection;
    }
}
