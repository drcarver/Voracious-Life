using System;

using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.Core.Interface;
using Voracious.Core.Model;

namespace Voracious.Control.ViewModel;

/// <summary>
/// User reviews a single book
/// </summary>
public partial class UserReviewViewModel : ObservableObject, IUserReviewCore
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private ResourceCore book;

    [ObservableProperty]
    private DateTime createDate = DateTime.UtcNow;

    [ObservableProperty]
    private DateTime mostRecentModificationDate = DateTime.Now;

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
