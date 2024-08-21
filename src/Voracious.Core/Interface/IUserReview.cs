using System;

namespace Voracious.Database;

/// <summary>
/// User reviews a single book
/// </summary>
public interface IUserReview
{
    int Id { get; set; }

    string BookId {  get; set; }

    DateTimeOffset CreateDate {  get; set; }

    DateTimeOffset MostRecentModificationDate {  get; set; }

    double NStars { get; set; }

    string Review {  get; set; }

    string Tags { get; set; }
}
