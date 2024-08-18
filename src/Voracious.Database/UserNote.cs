using Newtonsoft.Json;

using System.ComponentModel;
using System.Runtime.CompilerServices;

using Voracious.EbookReader;

namespace Voracious.Database;

/// <summary>
/// A user note is also the bookmark system. 
/// </summary>
public class UserNote : INotifyPropertyChanged, INotifyPropertyChanging
{
    private int id;
    private string bookId = "";
    private DateTimeOffset createDate = DateTimeOffset.Now;
    private DateTimeOffset mostRecentModificationDate = DateTimeOffset.Now;
    private string location = "";
    private string text = "";
    private string tags = "";
    private string icon = "";
    private string backgroundColor = "White";
    private string foregroundColor = "Black";
    private string selectedText = "";

    public int Id { get => id; set { if (id != value) { NotifyPropertyChanging(); id = value; NotifyPropertyChanged(); } } }

    /// <summary>
    /// BookId isn't the key because each book can have multiple notes.
    /// </summary>
    public string BookId { get => bookId; set { if (bookId != value) { NotifyPropertyChanging(); bookId = value; NotifyPropertyChanged(); } } }
    public DateTimeOffset CreateDate { get => createDate; set { if (createDate != value) { NotifyPropertyChanging(); createDate = value; NotifyPropertyChanged(); } } }
    public DateTimeOffset MostRecentModificationDate { get => mostRecentModificationDate; set { if (mostRecentModificationDate != value) { NotifyPropertyChanging(); mostRecentModificationDate = value; NotifyPropertyChanged(); } } }
    /// <summary>
    /// JSON version of the book location
    /// </summary>
    public string Location { get => location; set { if (location != value) { NotifyPropertyChanging(); location = value; NotifyPropertyChanged(); } } }
    public double LocationNumericValue
    {
        get
        {
            var location = LocationToBookLocatation();
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
    public bool AreSameSpot(UserNote external)
    {
        var retval = BookId == external.BookId
            && CreateDate == external.CreateDate
            && Location == external.Location
            ;
        return retval;

    }
    public bool AreEqual(UserNote note)
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

    public event PropertyChangedEventHandler PropertyChanged;
    public event PropertyChangingEventHandler PropertyChanging;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void NotifyPropertyChanging([CallerMemberName] string propertyName = "")
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }
    public BookLocation LocationToBookLocatation()
    {
#if IS_MIGRATION_PROJECT
        return null;
#else
        var location = JsonConvert.DeserializeObject<BookLocation>(Location);
        return location;
#endif
    }
#if IS_MIGRATION_PROJECT
    public class BookLocation
    {
        public double  HtmlPercent { get { return 1.0; } }
    }
#endif
    public string Text { get => text; set { if (text != value) { NotifyPropertyChanging(); text = value; NotifyPropertyChanged(); } } }
    public string Tags { get => tags; set { if (tags != value) { NotifyPropertyChanging(); tags = value; NotifyPropertyChanged(); } } }
    // space seperated? Kind of random text?
    public string Icon { get => icon; set { if (icon != value) { NotifyPropertyChanging(); icon = value; NotifyPropertyChanged(); } } }
    // in the Segoe UI symbol font
    public string BackgroundColor { get => backgroundColor; set { if (backgroundColor != value) { NotifyPropertyChanging(); backgroundColor = value; NotifyPropertyChanged(); } } }
    public string ForegroundColor { get => foregroundColor; set { if (foregroundColor != value) { NotifyPropertyChanging(); foregroundColor = value; NotifyPropertyChanged(); } } }
    public string SelectedText { get => selectedText; set { if (selectedText != value) { NotifyPropertyChanging(); selectedText = value; NotifyPropertyChanged(); } } }
}
