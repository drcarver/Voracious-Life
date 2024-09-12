using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.Extensions.Logging;
using Voracious.Database.Interface;

namespace Voracious.Reader.ViewModel;

public partial class MainPageViewModel : ObservableObject
{
    private IRdfReader Rdfreader { get; }

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
    }

    /// <summary>
    /// Update the gutenberg catalog
    /// </summary>
    /// <returns>The task for the catalog update</returns>
    [RelayCommand()]
    private async Task UpdateCatalog()
    {
        await Rdfreader.UpdateCatalogAsync();
    }
}
