using Voracious.Database;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Voracious.Controls
{
    public class UserNoteWithTitle
    {
        public UserNoteWithTitle(UserNote baseNote, bool displayTitle)
        {
            BaseNote = baseNote;
            DisplayTitle = displayTitle;
        }
        public UserNote BaseNote { get; set; }
        public BookData CorrespondingBook = null;

        public string Title 
        { 
            get 
            { 
                if (CorrespondingBook == null)
                {
                    var bookdb = BookDataContext.Get();
                    CorrespondingBook = CommonQueries.BookGet(bookdb, BookId);
                }
                return CorrespondingBook?.Title ?? BookId;
            } 
        }
        public bool DisplayTitle { get; set; }

        // Copy of data from UserNote
        public int Id { get { return BaseNote.Id; } }
        public string BookId { get { return BaseNote.BookId; } }
        public DateTimeOffset CreateDate { get { return BaseNote.CreateDate; } }
        public DateTimeOffset MostRecentModificationDate { get { return BaseNote.MostRecentModificationDate; } }
        public string Location { get { return BaseNote.Location; } }
        public string Text { get { return BaseNote.Text; } }
        public string Tags { get { return BaseNote.Tags; } }
        public string Icon { get { return BaseNote.Icon; } }
        public string BackgroundColor { get { return BaseNote.BackgroundColor; } }
        public string ForegroundColor { get { return BaseNote.ForegroundColor; } }
        public string SelectedText { get { return BaseNote.SelectedText; } }
    }
}
