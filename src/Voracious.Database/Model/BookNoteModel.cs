using System.Collections.ObjectModel;

using Voracious.Core.Interface;

namespace Voracious.Database.Model;

public class BookNoteModel : IBookNote
{
    public int Id { get; set; }
    public string BookId { get; set; }
    public ObservableCollection<IUserNote> Notes { get; set; }
}
