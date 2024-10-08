using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

using Voracious.Database.Interface;
using Voracious.Life.Interface;

namespace Voracious.Life.ViewModel;

public partial class MainPageViewModel : ObservableObject, IMainPage
{
    private ICardCatalog CardCatalog { get; }

    private ILogger<MainPageViewModel> logger;

    /// <summary>
    /// THe main page view model
    /// </summary>
    /// <param name="loggerFactory">The logger factory</param>
    /// <param name="cardCatalog">The CardCatalog</param>
    public MainPageViewModel(
        ILoggerFactory loggerFactory,
        ICardCatalog cardCatalog)
    {
        CardCatalog = cardCatalog;
        logger = loggerFactory.CreateLogger<MainPageViewModel>();
    }

    /// <summary>
    /// Is busy for the activity indicator
    /// </summary>
    [ObservableProperty]
    private bool isBusy = false;

    /// <summary>
    /// Update the card catalog
    /// </summary>
    /// <returns>The task for the catalog update</returns>
    [RelayCommand()]
    private async Task UpdateCatalog()
    {
        logger.LogInformation("Calling update catalog");
        IsBusy = true;
        await CardCatalog.UpdateCatalogAsync();
        IsBusy = false;
    }
}
