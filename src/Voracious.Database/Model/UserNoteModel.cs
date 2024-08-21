using Voracious.Core.Interface;

namespace Voracious.Database.Model;

public class UserNoteModel : IUserNote
{
    public int Id { get; set; }
    public string BookId { get; set; }
    public DateTimeOffset CreateDate { get; set; }
    public DateTimeOffset MostRecentModificationDate { get; set; }
    public string Location { get; set; }
    public string Text { get; set; }
    public string Tags { get; set; }
    public string Icon { get; set; }
    public string BackgroundColor { get; set; }
    public string ForegroundColor { get; set; }
    public string SelectedText { get; set; }
    public double LocationNumericValue { get; }
}
