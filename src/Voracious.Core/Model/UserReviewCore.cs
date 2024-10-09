using System;

using Voracious.Core.Interface;

namespace Voracious.Core.Model;

/// <summary>
/// User reviews a single book
/// </summary>
public class UserReviewCore : IUserReviewCore
{
    public int Id { get; set; }

    public ResourceCore Book { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime MostRecentModificationDate { get; set; }

    public double NStars { get; set; }

    public string Tags { get; set; }
}
