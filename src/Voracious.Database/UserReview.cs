using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Voracious.Database;

/// <summary>
/// User reviews a single book
/// </summary>
public class UserReview : INotifyPropertyChanged, INotifyPropertyChanging
{
    private int id;
    private string bookId;
    private DateTimeOffset createDate = DateTimeOffset.UtcNow;
    private DateTimeOffset mostRecentModificationDate = DateTimeOffset.Now;
    private double nStars = 0;
    private string review;
    private string tags;

    public int Id { get => id; set { if (id != value) { NotifyPropertyChanging(); id = value; NotifyPropertyChanged(); } } }
    public UserReview() { }
    public UserReview(string id) { BookId = id; }
    public string BookId { get => bookId; set { if (bookId != value) { NotifyPropertyChanging(); bookId = value; NotifyPropertyChanged(); } } }
    public DateTimeOffset CreateDate { get => createDate; set { if (createDate != value) { NotifyPropertyChanging(); createDate = value; NotifyPropertyChanged(); } } }
    public DateTimeOffset MostRecentModificationDate { get => mostRecentModificationDate; set { if (mostRecentModificationDate != value) { NotifyPropertyChanging(); mostRecentModificationDate = value; NotifyPropertyChanged(); } } }


    public double NStars { get => nStars; set { if (nStars != value) { NotifyPropertyChanging(); nStars = value; NotifyPropertyChanged(); } } }
    public string Review { get => review; set { if (review != value) { NotifyPropertyChanging(); review = value; NotifyPropertyChanged(); } } }
    public string Tags { get => tags; set { if (tags != value) { NotifyPropertyChanging(); tags = value; NotifyPropertyChanged(); } } }

    // space seperated? It's knd of random text

    public bool IsNotSet
    {
        get
        {
            var retval = NStars == 0 && string.IsNullOrEmpty(Review) && string.IsNullOrEmpty(Tags);
            return retval;
        }
    }

    /// <summary>
    /// Merge the 'external' review into this review. The newest data wins. Returns >0 iff some data was updated.
    /// Will update the MostRecentModificationDate only if there are changes.
    /// </summary>
    /// <param name="external"></param>
    /// <returns>0 if there are no changes, 1 or more for the number of changes. </returns>
    public int Merge(UserReview external)
    {
        int retval = 0;
        if (external != null && external.MostRecentModificationDate > MostRecentModificationDate)
        {
            if (external.NStars != NStars)
            {
                NStars = external.NStars;
                retval++;
            }
            if (external.Review != Review)
            {
                Review = external.Review;
                retval++;
            }
            if (external.Tags != Tags)
            {
                Tags = external.Tags;
                retval++;
            }
            if (retval > 0)
            {
                MostRecentModificationDate = external.MostRecentModificationDate;
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
    public override string ToString()
    {
        return $"{Review.Substring(0, Math.Min(Review.Length, 200))} for {BookId}";
    }
}
