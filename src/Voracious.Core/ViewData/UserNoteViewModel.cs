using System;

using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.Core.Model;

namespace Voracious.Core.ViewModel;

/// <summary>
/// A user note is also the bookmark system. 
/// </summary>
public partial class UserNoteViewModel : ObservableObject
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private string bookId = "";

    [ObservableProperty]
    private DateTimeOffset createDate = DateTimeOffset.Now;

    [ObservableProperty]
    private DateTimeOffset mostRecentModificationDate = DateTimeOffset.Now;

    [ObservableProperty]
    private string location = "";

    [ObservableProperty]
    private string text = "";

    [ObservableProperty]
    private string tags = "";

    [ObservableProperty]
    private string icon = "";

    [ObservableProperty]
    private string backgroundColor = "White";

    [ObservableProperty]
    private string foregroundColor = "Black";

    [ObservableProperty]
    private string selectedText = "";

    /// <summary>
    /// JSON version of the book location
    /// </summary>
    public double LocationNumericValue
    {
        get
        {
            var location = LocationToBookLocation();
            var retval = location?.HtmlPercent ?? -1.0; // not a percent.
            return retval;
        }
    }

    /// <summary>
    /// Two kinds of equal: equal enough that if they are different, the newer one should
    /// take priority, and exactly equal. This one is the equal enough one and uses data
    /// that doesn't change from time to another. The 'id' number might be different from
    /// one machine to another.
    /// </summary>
    /// <param name="external"></param>
    /// <returns></returns>
    public bool AreSameSpot(UserNoteViewModel external)
    {
        var retval = BookId == external.BookId
            && CreateDate == external.CreateDate
            && Location == external.Location
            ;
        return retval;

    }
    public bool AreEqual(UserNoteViewModel note)
    {
        var retval = BookId == note.BookId
            && CreateDate == note.CreateDate
            && MostRecentModificationDate == note.MostRecentModificationDate
            && Location == note.Location
            && Text == note.Text
            && Tags == note.Tags
            && Icon == note.Icon
            && BackgroundColor == note.BackgroundColor
            && ForegroundColor == note.ForegroundColor
            && SelectedText == note.SelectedText
            ;
        return retval;
    }

    public BookLocation LocationToBookLocation()
    {
        return null;
    }
}
