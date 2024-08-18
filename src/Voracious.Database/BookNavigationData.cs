using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Voracious.Database;

/// <summary>
/// In progress -- is this what I really want?
/// </summary>
public partial class BookNavigationData : INotifyPropertyChanged, INotifyPropertyChanging
{
    private int id;
    private string bookId;
    private int nCatalogViews;
    private int nSwipeRight;
    private int nSwipeLeft;
    private int nReading;
    private int nSpecificSelection;
    private string currSpot = "";
    private UserStatusEnum currStatus = UserStatusEnum.NoStatus;
    private DateTimeOffset timeMarkedDone = DateTimeOffset.MinValue;
    private DateTimeOffset firstNavigationDate = DateTimeOffset.Now;
    private DateTimeOffset mostRecentNavigationDate = DateTimeOffset.Now;

    public int Id { get => id; set { if (id != value) { NotifyPropertyChanging(); id = value; NotifyPropertyChanged(); } } }
    public string BookId { get => bookId; set { if (bookId != value) { NotifyPropertyChanging(); bookId = value; NotifyPropertyChanged(); } } }
    public int NCatalogViews { get => nCatalogViews; set { if (nCatalogViews != value) { NotifyPropertyChanging(); nCatalogViews = value; Touch(); NotifyPropertyChanged(); } } }
    public int NSwipeRight { get => nSwipeRight; set { if (nSwipeRight != value) { NotifyPropertyChanging(); nSwipeRight = value; Touch(); NotifyPropertyChanged(); } } }

    // "approve"
    public int NSwipeLeft { get => nSwipeLeft; set { if (NSwipeLeft != value) { NotifyPropertyChanging(); nSwipeLeft = value; Touch(); NotifyPropertyChanged(); } } }

    // "disapprove"
    public int NReading { get => nReading; set { if (nReading != value) { NotifyPropertyChanging(); nReading = value; Touch(); NotifyPropertyChanged(); } } }
    public int NSpecificSelection { get => nSpecificSelection; set { if (nSpecificSelection != value) { NotifyPropertyChanging(); nSpecificSelection = value; Touch(); NotifyPropertyChanged(); } } }

    // number of times the book was specifically selected

    public string CurrSpot { get => currSpot; set { if (currSpot != value) { NotifyPropertyChanging(); currSpot = value; Touch(); NotifyPropertyChanged(); } } }

    public UserStatusEnum CurrStatus { get => currStatus; set { if (currStatus != value) { NotifyPropertyChanging(); currStatus = value; Touch(); NotifyPropertyChanged(); } } }
    public bool IsDone { get { return CurrStatus == UserStatusEnum.Done || CurrStatus == UserStatusEnum.Abandoned; } }
    public DateTimeOffset TimeMarkedDone
    {
        get => timeMarkedDone;
        set { if (timeMarkedDone != value) { NotifyPropertyChanging(); timeMarkedDone = value; Touch(); NotifyPropertyChanged(); } }
    }
    public DateTimeOffset FirstNavigationDate { get => firstNavigationDate; set { if (firstNavigationDate != value) { NotifyPropertyChanging(); firstNavigationDate = value; NotifyPropertyChanged(); } } }
    public DateTimeOffset MostRecentNavigationDate { get => mostRecentNavigationDate; set { if (mostRecentNavigationDate != value) { NotifyPropertyChanging(); mostRecentNavigationDate = value; NotifyPropertyChanged(); } } }
    public void Touch()
    {
        MostRecentNavigationDate = DateTimeOffset.Now;
    }

    public int Merge(BookNavigationData external)
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

        return retval;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public event PropertyChangingEventHandler PropertyChanging;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void NotifyPropertyChanging([CallerMemberName] string propertyName = "")
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }
}
