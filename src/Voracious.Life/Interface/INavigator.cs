using Voracious.Control.ViewModel;
using Voracious.EPub.Interface;
using Voracious.Life.Enum;
using Voracious.Life.Model;

namespace Voracious.Life.Interface;

public interface INavigator
{
    IBookHandler? MainBookHandler { get; set; }

    void AddNavigateTo(NavigateControlIdEnum id, INavigateTo navigateTo);
    void AddSelectTo(NavigateControlIdEnum id, ISelectTo selectTo);
    void AddSetAppColor(NavigateControlIdEnum id, ISetAppColors setColor);
    void AddSimpleBookHandler(NavigateControlIdEnum id, ISimpleBookHandler simple);
    bool DisplayBook(NavigateControlIdEnum id, ResourceViewModel bookData, BookLocation location = null);
    void RemoveSelectTo(NavigateControlIdEnum id);
    void SetAppColors(Color bg, Color fg);
    void UpdatedNotes(NavigateControlIdEnum id);
    void UpdateProjectRome(NavigateControlIdEnum sourceId, BookLocation location);
    void UserNavigatedTo(NavigateControlIdEnum sourceId, BookLocation location);
    void UserSelected(NavigateControlIdEnum sourceId, string selection);
}