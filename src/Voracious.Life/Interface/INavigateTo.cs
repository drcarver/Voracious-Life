using Voracious.Life.Enum;
using Voracious.Life.Model;

namespace Voracious.Life.Interface;

public interface INavigateTo
{
    /// <summary>
    /// Individual controls implement this; when the user navigates (e.g., from the chapter list),
    /// each control learns about it because their implementation of NavigateTo is called.
    /// </summary>
    /// <param name="sourceId"></param>
    /// <param name="location"></param>
    void NavigateTo(NavigateControlIdEnum sourceId, BookLocation location);
}
