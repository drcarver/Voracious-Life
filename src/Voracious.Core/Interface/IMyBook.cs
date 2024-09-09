using Voracious.Core.ViewModel;

namespace Voracious.Core.Interface;

/// <summary>
/// Each book in my book books is linked to the main catalog
/// </summary>
public interface IMyBook
{
    int Id { get; set; }

    ResourceViewModel? BookId  { get; set; }
}
