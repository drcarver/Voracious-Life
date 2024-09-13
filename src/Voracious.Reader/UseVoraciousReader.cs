using CommunityToolkit.Maui;

using Microsoft.Extensions.DependencyInjection;

using Voracious.Reader.Interface;
using Voracious.Reader.View;
using Voracious.Reader.ViewModel;

namespace Voracious.Database;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseVoraciousReader(this IServiceCollection collection)
    {
        collection
            .AddTransient<IMainPage, MainPageViewModel>()
            .AddTransientWithShellRoute<MainPage, MainPageViewModel>(nameof(MainPage))
            .AddTransientWithShellRoute<PDFViewerPage, PdfViewerViewModel>(nameof(PDFViewerPage))
            ;

        return collection;
    }
}
