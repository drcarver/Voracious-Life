using CommunityToolkit.Maui;

using Voracious.Life.Interface;
using Voracious.Life.View;
using Voracious.Life.ViewModel;

namespace Voracious.Life;

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
