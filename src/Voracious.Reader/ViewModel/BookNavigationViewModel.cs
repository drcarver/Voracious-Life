using System;

using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.RDF.Enum;
using Voracious.RDF.Interface;

namespace Voracious.Core.ViewModel;

public partial class BookNavigationViewModel : ObservableObject, IBookNavigation
{
    [ObservableProperty]
    private int id;

    [ObservableProperty] 
    private IResource resource;

    [ObservableProperty]
    private DateTime mostRecentNavigationDate = DateTime.Now;

    [ObservableProperty]
    private int numberOfCatalogViews;

    [ObservableProperty]
    private int numberOfSwipeRight;

    [ObservableProperty]
    private int numberOfSwipeLeft;

    [ObservableProperty]
    private int numberReading;

    [ObservableProperty]
    private int numberSpecificSelection;

    [ObservableProperty]
    private string currentSpot = "";

    [ObservableProperty]
    private UserStatusEnum currentStatus = UserStatusEnum.NoStatus;

    [ObservableProperty]
    private DateTime timeMarkedDone = DateTime.MinValue;

    [ObservableProperty]
    private DateTime firstNavigationDate = DateTime.Now;

    [ObservableProperty]
    private bool isDone = false;
}
