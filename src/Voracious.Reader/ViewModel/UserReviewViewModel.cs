using System;

using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.RDF.Interface;

namespace Voracious.RDF.ViewModel;

/// <summary>
/// User reviews a single book
/// </summary>
public partial class UserReviewViewModel : ObservableObject, IUserReview
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private ResourceViewModel book;

    [ObservableProperty]
    private DateTimeOffset createDate = DateTimeOffset.UtcNow;

    [ObservableProperty]
    private DateTimeOffset mostRecentModificationDate = DateTimeOffset.Now;

    [ObservableProperty]
    private double nStars = 0;

    [ObservableProperty]
    private string tags;

    public UserReviewViewModel() { }

    public bool IsNotSet
    {
        get
        {
            var retval = NStars == 0 && string.IsNullOrEmpty(Tags);
            return retval;
        }
    }
}
