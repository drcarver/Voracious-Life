using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;

using Voracious.Database.Interface;
using Voracious.Reader.Interface;

namespace Voracious.Reader.ViewModel;

public partial class MainPageViewModel : ObservableObject, IMainPage
{
    private IRdfReader Rdfreader { get; }

    private ILogger<MainPageViewModel> logger;

    /// <summary>
    /// THe main page view model
    /// </summary>
    /// <param name="loggerFactory">The logger factory</param>
    /// <param name="rdfreader">The rdfreader</param>
    public MainPageViewModel(
        ILoggerFactory loggerFactory,
        IRdfReader rdfreader)
    {
        Rdfreader = rdfreader;
        logger = loggerFactory.CreateLogger<MainPageViewModel>();
    }

    /// <summary>
    /// Is busy for the activity indicator
    /// </summary>
    [ObservableProperty]
    private bool isBusy = false;

    /// <summary>
    /// Update the gutenberg catalog
    /// </summary>
    /// <returns>The task for the catalog update</returns>
    [RelayCommand()]
    private async Task UpdateCatalog()
    {
        logger.LogInformation("Calling update catalog");
        IsBusy = true;
        await Rdfreader.UpdateCatalogAsync();
        IsBusy = false;
    }
}
