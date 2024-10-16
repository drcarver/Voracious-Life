using CommunityToolkit.Maui;

using Voracious.Control.Interface;
using Voracious.Control.ViewModel;
using Voracious.EPub;
using Voracious.Life.Interface;
using Voracious.Life.View;

namespace Voracious.Life;

public static class ServiceCollectionExtension
{
    public static IServiceCollection UseVoraciousReader(this IServiceCollection collection)
    {
        collection
            .AddTransient<IMainPage, MainPageViewModel>()
            .AddTransientWithShellRoute<MainPage, MainPageViewModel>(nameof(MainPage))
            .AddSingleton<INavigator, Navigator>()
            ;

        return collection;
    }
}
