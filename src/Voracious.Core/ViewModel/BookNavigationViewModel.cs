using System;

using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.Core.Enum;
using Voracious.Core.Interface;

namespace Voracious.Core.ViewModel;

public partial class BookNavigationViewModel : ObservableObject, IBookNavigation
{
    [ObservableProperty]
    private int id;

    [ObservableProperty] 
    private ResourceViewModel book;

    [ObservableProperty]
    private DateTimeOffset mostRecentNavigationDate = DateTimeOffset.Now;

    [ObservableProperty]
    private int nCatalogViews;

    [ObservableProperty]
    private int nSwipeRight;

    [ObservableProperty]
    private int nSwipeLeft;

    [ObservableProperty]
    private int nReading;

    [ObservableProperty]
    private int nSpecificSelection;

    [ObservableProperty]
    private string currSpot = "";

    [ObservableProperty]
    private UserStatusEnum currStatus = UserStatusEnum.NoStatus;

    [ObservableProperty]
    private DateTimeOffset timeMarkedDone = DateTimeOffset.MinValue;

    [ObservableProperty]
    private DateTimeOffset firstNavigationDate = DateTimeOffset.Now;

    [ObservableProperty]
    private bool isDone = false;
}
