using Voracious.Life.Enum;

namespace Voracious.Life.Interface;

public interface ISelectTo
{
    /// <summary>
    /// Individual controls (like WebSeach) implement this; when the user selects text in the book (for example),
    /// registered controls will pick it up and display it. Note that it's possible that the control
    /// won't do the search if it's not visible.
    /// </summary>
    /// <param name="sourceId"></param>
    /// <param name="selected"></param>
    void SelectTo(NavigateControlIdEnum sourceId, string selected);
}
