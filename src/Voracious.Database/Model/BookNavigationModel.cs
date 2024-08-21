using Voracious.Core.Enum;
using Voracious.Core.Interface;

namespace Voracious.Database.Model;

public class BookNavigationModel : IBookNavigation
{
    public int Id { get; set; }
    public string BookId { get; set; }
    public DateTimeOffset MostRecentNavigationDate { get; set; }
    public int NCatalogViews { get; set; }
    public int NSwipeRight { get; set; }
    public int NSwipeLeft { get; set; }
    public int NReading { get; set; }
    public int NSpecificSelection { get; set; }
    public string CurrSpot { get; set; }
    public UserStatusEnum CurrStatus { get; set; }
    public DateTimeOffset TimeMarkedDone { get; set; }
    public DateTimeOffset FirstNavigationDate { get; set; }
    public bool IsDone { get; set; }
}
