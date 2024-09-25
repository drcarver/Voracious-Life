using System.ComponentModel;

using Microsoft.Extensions.DependencyInjection;

using Voracious.Core.ViewModel;
using Voracious.RDF.Interface;
using Voracious.RDF.ViewModel;

namespace Voracious.Core;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseVoraciousRDF(this IServiceCollection collection)
    {
        collection
            .AddTransient<IBookNavigation, BookNavigationViewModel>()
            .AddTransient<IResource, ResourceViewModel>()
            .AddTransient<IFileFormat, FileFormatViewModel>()
            .AddTransient<ICreator, CreatorViewModel>()
            .AddTransient<IUserNote, UserNoteViewModel>()
            .AddTransient<IUserReview, UserReviewViewModel>()
             ;
        return collection;
    }
}
