using System;

using Voracious.RDF.Enum;

namespace Voracious.RDF.Interface;

public interface IBookNavigation
{
    int Id { get; set; }
    IResource Resource { get; set; }
    DateTime MostRecentNavigationDate { get; set; }
    int NumberOfCatalogViews { get; set; }
    int NumberOfSwipeRight { get; set; }
    int NumberOfSwipeLeft { get; set; }
    int NumberReading { get; set; }
    int NumberSpecificSelection { get; set; }
    string CurrentSpot { get; set; }
    UserStatusEnum CurrentStatus { get; set; }
    DateTime TimeMarkedDone { get; set; }
    DateTime FirstNavigationDate { get; set; }
    bool IsDone { get; set; }
}
