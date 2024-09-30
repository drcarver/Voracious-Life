using CommunityToolkit.Maui;

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
            ;

        return collection;
    }
}
