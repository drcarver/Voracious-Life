using System;

using Voracious.Core.Enum;
using Voracious.Core.ViewModel;

namespace Voracious.Core.Interface;

public interface IBookNavigation
{
    int Id { get; set; }

    ResourceViewModel? Book { get; set; }

    DateTimeOffset MostRecentNavigationDate { get; set; }

    int NCatalogViews { get; set; }

    int NSwipeRight { get; set; }

    int NSwipeLeft { get; set; }

    int NReading { get; set; }

    int NSpecificSelection { get; set; }

    string CurrSpot { get; set; }

    UserStatusEnum CurrStatus { get; set; }

    DateTimeOffset TimeMarkedDone { get; set; }

    DateTimeOffset FirstNavigationDate { get; set; }

    bool IsDone { get; set; }
}
