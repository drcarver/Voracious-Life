using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.DependencyInjection;

using Voracious.Core.Interface;
using Voracious.Core.ViewModel;
using Voracious.Database;

namespace Voracious.Core;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseVoraciousCore(this IServiceCollection collection)
    {
        collection
            .AddTransient<IBookNavigation, BookNavigationViewModel>()
            .AddTransient<IBookNote, BookNoteViewModel>()
            .AddTransient<IBook, BookViewModel>()
            .AddTransient<IDownloadData, DownloadDataViewModel>()
            .AddTransient<IFilenameAndFormatData, FilenameAndFormatDataViewModel>()
            .AddTransient<IPerson, PersonViewModel>()
            .AddTransient<IUserNote, UserNoteViewModel>()
            .AddTransient<IUserReview, UserReviewViewModel>()
             ;
        return collection;
    }
}
