using System;
using System.ComponentModel.DataAnnotations;

using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.Core.Enum;
using Voracious.Core.Interface;

namespace Voracious.Core.ViewModel;

public partial class BookNavigationViewModel : ObservableObject, IBookNavigation
{
    [ObservableProperty]
    [property: Key]
    private int id;

    [ObservableProperty]
    private string bookId;

    partial void OnBookIdChanged(string value)
    {
        Touched();
    }

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

    partial void OnTimeMarkedDoneChanged(DateTimeOffset oldValue, DateTimeOffset newValue)
    {
        Touched();
    }

    [ObservableProperty]
    private DateTimeOffset firstNavigationDate = DateTimeOffset.Now;

    [ObservableProperty]
    private bool isDone = false;

    /// <summary>
    /// Has the object been updated?
    /// </summary>
    public void Touched()
    {
        MostRecentNavigationDate = DateTimeOffset.Now;
        if (CurrStatus == UserStatusEnum.Done || CurrStatus == UserStatusEnum.Abandoned)
        {
            if (!IsDone)
            {
                IsDone = true;
                TimeMarkedDone = DateTimeOffset.Now;
            }
        }
    }

    /// <summary>
    /// Merge two BookNavigationData objects
    /// </summary>
    /// <param name="external">BookNavigationData tp be merged</param>
    /// <returns>The merge count</returns>
    public bool Merge(BookNavigationViewModel external)
    {
        int retval = 0;
        if (external != null && external.MostRecentNavigationDate > MostRecentNavigationDate)
        {
            if (external.NCatalogViews != NCatalogViews)
            {
                NCatalogViews = external.NCatalogViews;
                retval++;
            }
            if (external.NSwipeRight != NSwipeRight)
            {
                NSwipeRight = external.NSwipeRight;
                retval++;
            }
            if (external.NSwipeLeft != NSwipeLeft)
            {
                NSwipeLeft = external.NSwipeLeft;
                retval++;
            }
            if (external.NReading != NReading)
            {
                NReading = external.NReading;
                retval++;
            }
            if (external.NSpecificSelection != NSpecificSelection)
            {
                NSpecificSelection = external.NSpecificSelection;
                retval++;
            }
            if (external.CurrSpot != CurrSpot)
            {
                CurrSpot = external.CurrSpot;
                retval++;
            }
            if (external.CurrStatus != CurrStatus)
            {
                CurrStatus = external.CurrStatus;
                retval++;
            }
            if (external.TimeMarkedDone > TimeMarkedDone)
            {
                TimeMarkedDone = external.TimeMarkedDone;
                retval++;
            }
            if (external.FirstNavigationDate != FirstNavigationDate)
            {
                FirstNavigationDate = external.FirstNavigationDate;
                retval++;
            }
            if (retval > 0)
            {
                MostRecentNavigationDate = external.MostRecentNavigationDate;
            }
        }

        return retval == 0;
    }
}
