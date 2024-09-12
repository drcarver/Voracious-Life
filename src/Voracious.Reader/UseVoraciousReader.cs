using CommunityToolkit.Maui;

using Microsoft.Extensions.DependencyInjection;

using Voracious.Reader.View;
using Voracious.Reader.ViewModel;

namespace Voracious.Database;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseVoraciousReader(this IServiceCollection collection)
    {
        collection
            .AddTransientWithShellRoute<MainPage, MainPageViewModel>(nameof(MainPageViewModel))
            .AddTransientWithShellRoute<PDFViewerPage, PdfViewerViewModel>(nameof(PDFViewerPage))
            ;

        return collection;
    }
}
