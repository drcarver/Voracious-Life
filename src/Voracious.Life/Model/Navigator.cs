using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using Voracious.Control.ViewModel;
using Voracious.Core.Enum;
using Voracious.EPub.Interface;
using Voracious.Life.Enum;
using Voracious.Life.Interface;
using Voracious.Life.Model;

namespace Voracious.EPub;

public partial class Navigator : INavigator
{
    private readonly ILogger Logger;

    public Navigator(ILoggerFactory loggerFactory)
    {
        Logger = loggerFactory.CreateLogger<Navigator>();
    }

    /// <summary>
    /// The book handler is the class+object that knows all the details of 
    /// the book
    /// </summary>
    public IBookHandler? MainBookHandler { get; set; } = null;

    /// <summary>
    /// Setup routine called just a few times at app startup. Tells the navigator which
    /// displays are which.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="navigateTo"></param>
    public void AddNavigateTo(NavigateControlIdEnum id, INavigateTo navigateTo)
    {
        if (NavigateTos.ContainsKey(id))
        {
            NavigateTos[id] = navigateTo;
        }
        else
        {
            NavigateTos.Add(id, navigateTo);
        }
    }

    public void AddSelectTo(NavigateControlIdEnum id, ISelectTo selectTo)
    {
        SelectTos.Add(id, selectTo);
    }
    public void RemoveSelectTo(NavigateControlIdEnum id)
    {
        if (SelectTos.ContainsKey(id))
        {
            SelectTos.Remove(id);
        }
    }

    public void AddSetAppColor(NavigateControlIdEnum id, ISetAppColors setColor)
    {
        AppColors[id] = setColor;
    }

    public void AddSimpleBookHandler(NavigateControlIdEnum id, ISimpleBookHandler simple)
    {
        SimpleBookHandlers.Add(id, simple);
    }

    public bool DisplayBook(NavigateControlIdEnum id, ResourceViewModel bookData, BookLocation location = null)
    {
        if (MainBookHandler == null) return false;

        // Is the book actually downloaded?
        if (bookData.FileStatus != FileStatusEnum.Downloaded)
        {
            // TODO: download the book
            return false;
        }

        MainBookHandler.DisplayBook(bookData, location);
        foreach (var item in SimpleBookHandlers)
        {
            // No hairpin selects
            if (item.Key != id)
            {
                item.Value.DisplayBook(bookData, location);
            }
        }

        return true;
    }

    public void SetAppColors(Color bg, Color fg)
    {
        foreach (var (item, value) in AppColors)
        {
            value.SetAppColors(bg, fg);
        }
    }

    public void UpdatedNotes(NavigateControlIdEnum id)
    {
        foreach (var item in SimpleBookHandlers)
        {
            // No hairpin selects
            if (item.Key != id)
            {
                if (item.Key == NavigateControlIdEnum.NoteListDisplay || item.Key == NavigateControlIdEnum.BookSearchDisplay)
                {
                    item.Value.DisplayBook(null, null); // refreshes the book
                }
            }
        }
    }

    /// <summary>
    /// Called (often by the main display) when the user selects some book text.
    /// AFAICT, there's no reason for anyone else to call this. The end result is
    /// that e.g. the web search gets the selected text and does a web search.
    /// </summary>
    /// <param name="sourceId"></param>
    /// <param name="selection"></param>
    public void UserSelected(NavigateControlIdEnum sourceId, string selection)
    {
        foreach (var (id, control) in SelectTos)
        {
            if (id != sourceId)
            {
                control.SelectTo(sourceId, selection);
            }
        }

    }
    /// <summary>
    /// Called by any of the displays when the user has picked a place to navigate to.
    /// Is never called automatically. The place is a place inside the already-viewed ebook.
    /// </summary>
    /// <param name="sourceId"></param>
    /// <param name="location"></param>
    public void UserNavigatedTo(NavigateControlIdEnum sourceId, BookLocation location)
    {
        Logger.LogInformation($"UserNavigatedTo({location})");
        foreach (var (id, control) in NavigateTos)
        {
            if (id != sourceId)
            {
                control.NavigateTo(sourceId, location);
            }
        }
    }
    public void UpdateProjectRome(NavigateControlIdEnum sourceId, BookLocation location)
    {
        foreach (var (id, control) in NavigateTos)
        {
            if (id != sourceId && id == NavigateControlIdEnum.ProjectRome)
            {
                control.NavigateTo(sourceId, location);
            }
        }
    }

    Dictionary<NavigateControlIdEnum, ISetAppColors> AppColors = [];
    Dictionary<NavigateControlIdEnum, INavigateTo> NavigateTos = [];
    Dictionary<NavigateControlIdEnum, ISelectTo> SelectTos = [];
    Dictionary<NavigateControlIdEnum, ISimpleBookHandler> SimpleBookHandlers = [];
}
