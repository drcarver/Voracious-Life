using System;

using Voracious.Core.Enum;
using Voracious.Core.ViewModel;

namespace Voracious.Core.Interface;

public interface IBookNavigationData
{
    int Id { get; set; }

    string BookId { get;set; }

    DateTimeOffset MostRecentNavigationDate { get; set; }

    int NCatalogViews { get; set; }

    int NSwipeRight {  get; set; }

    int NSwipeLeft { get; set; }

    int NReading { get; set; }

    int NSpecificSelection { get; set; }

    string CurrSpot { get; set; }

    UserStatusEnum CurrStatus { get; set; }

    DateTimeOffset TimeMarkedDone {  get; set; }

    DateTimeOffset FirstNavigationDate { get; set; }

    bool IsDone { get; set; }

    void Touched();

    /// <summary>
    /// Merge two BookNavigationData objects
    /// </summary>
    /// <param name="external">BookNavigationData tp be merged</param>
    /// <returns>The merge count</returns>
    bool Merge(BookNavigationDataViewModel external);
}
