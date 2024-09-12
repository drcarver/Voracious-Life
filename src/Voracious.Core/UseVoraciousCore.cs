using Microsoft.Extensions.DependencyInjection;

using Voracious.Core.Interface;
using Voracious.Core.Model;
using Voracious.Core.ViewModel;

namespace Voracious.Core;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseVoraciousCore(this IServiceCollection collection)
    {
        collection
            .AddTransient<IBookNavigation, BookNavigationViewModel>()
            .AddTransient<IResource, ResourceViewModel>()
            .AddTransient<IFilenameAndFormatData, FilenameAndFormatDataViewModel>()
            .AddTransient<IPerson, PersonViewModel>()
            .AddTransient<IUserNote, UserNoteViewModel>()
            .AddTransient<IUserReview, UserReviewViewModel>()
             ;
        return collection;
    }
}
