using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Voracious.Core.ViewModel;

public partial class BookNoteViewModel : ObservableObject
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private string bookId;

    [ObservableProperty]
    private ObservableCollection<UserNoteViewModel> notes = [];

    /// <summary>
    /// Find the equal-enough matching note by index. Return -1 if not found.
    /// </summary>
    /// <param name="external"></param>
    /// <returns></returns>
    private int FindSameSpot(UserNoteViewModel external)
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
    public int Merge(BookNoteViewModel external)
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
