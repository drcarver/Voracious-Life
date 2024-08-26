using System;
using System.ComponentModel.DataAnnotations;

using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.Core.Interface;

namespace Voracious.Core.ViewModel;

/// <summary>
/// User reviews a single book
/// </summary>
public partial class UserReviewViewModel : ObservableObject, IUserReview
{
    [ObservableProperty]
    [property: Key]
    private int id;

    [ObservableProperty]
    private string bookId;

    [ObservableProperty]
    private DateTimeOffset createDate = DateTimeOffset.UtcNow;

    [ObservableProperty]
    private DateTimeOffset mostRecentModificationDate = DateTimeOffset.Now;

    [ObservableProperty]
    private double nStars = 0;

    [ObservableProperty]
    private string review;

    [ObservableProperty]
    private string tags;

    public UserReviewViewModel() { }
    public UserReviewViewModel(string id) { BookId = id; }

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
    public int Merge(UserReviewViewModel external)
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

    public override string ToString()
    {
        return $"{Review.Substring(0, Math.Min(Review.Length, 200))} for {BookId}";
    }
}
