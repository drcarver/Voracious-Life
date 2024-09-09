using System;

using Voracious.Core.ViewModel;

namespace Voracious.Core.Interface;

/// <summary>
/// User reviews a single book
/// </summary>
public interface IUserReview
{
    int Id { get; set; }

    ResourceViewModel Book { get; set; }

    DateTimeOffset CreateDate { get; set; }

    DateTimeOffset MostRecentModificationDate { get; set; }

    double NStars { get; set; }

    string Tags { get; set; }
}
