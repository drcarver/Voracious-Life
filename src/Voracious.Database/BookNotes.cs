using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Voracious.Database;

public class BookNotes : INotifyPropertyChanged
{
    private int id;
    private string bookId;

    public int Id { get => id; set { if (id != value) { NotifyPropertyChanging(); id = value; NotifyPropertyChanged(); } } }
    public string BookId { get => bookId; set { if (bookId != value) { NotifyPropertyChanging(); bookId = value; NotifyPropertyChanged(); } } }
    public ObservableCollection<UserNote> Notes { get; set; } = new ObservableCollection<UserNote>();

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

    /// <summary>
    /// Find the equal-enough matching note by index. Return -1 if not found.
    /// </summary>
    /// <param name="external"></param>
    /// <returns></returns>
    private int FindSameSpot(UserNote external)
    {
        for (int i = 0; i < Notes.Count; i++)
        {
            if (Notes[i].AreSameSpot(external))
            {
                return i;
            }
        }
        return -1;
    }


    /// <summary>
    /// Merge the 'external' review into this review. The newest data wins. Returns >0 iff some data was updated.
    /// Will update the MostRecentModificationDate only if there are changes.
    /// </summary>
    /// <param name="external"></param>
    /// <returns>0 if there are no changes, 1 or more for the number of changes. </returns>
    public int Merge(BookNotes external)
    {
        int retval = 0;
        if (external != null)
        {
            // For each note in the external, see if it's already present here.
            foreach (var externalNote in external.Notes)
            {
                var index = FindSameSpot(externalNote);
                if (index < 0)
                {
                    // Just add it in
                    externalNote.Id = 0; // set to zero for EF
                    Notes.Add(externalNote);
                    retval++;
                }
                else
                {
                    var note = Notes[index];
                    if (note.AreEqual(externalNote))
                    {
                        ; // notes are exactly equal; no need to update anything.
                    }
                    else if (note.MostRecentModificationDate > externalNote.MostRecentModificationDate)
                    {
                        ; // the current note is already up to date
                    }
                    else // external note is newer; replace the old note
                    {
                        Notes.RemoveAt(index);
                        externalNote.Id = 0; // set to zero for EF
                        Notes.Insert(index, externalNote);
                        retval++;
                    }
                }
            }
        }
        return retval;
    }
}
