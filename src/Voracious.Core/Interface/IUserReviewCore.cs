using System;

using Voracious.Core.Model;

namespace Voracious.Core.Interface;

/// <summary>
/// User reviews a single book
/// </summary>
public interface IUserReviewCore
{
    int Id { get; set; }

    ResourceCore Book { get; set; }

    DateTime CreateDate { get; set; }

    DateTime MostRecentModificationDate { get; set; }

    double NStars { get; set; }

    string Tags { get; set; }
}
