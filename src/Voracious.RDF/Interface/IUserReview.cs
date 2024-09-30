using System;

namespace Voracious.RDF.Interface;

/// <summary>
/// User reviews a single book
/// </summary>
public interface IUserReview
{
    int Id { get; set; }

    IResource Book { get; set; }

    DateTimeOffset CreateDate { get; set; }

    DateTimeOffset MostRecentModificationDate { get; set; }

    double NStars { get; set; }

    string Tags { get; set; }
}
