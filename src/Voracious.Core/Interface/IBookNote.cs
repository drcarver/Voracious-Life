using System.Collections.ObjectModel;

using Voracious.Core.ViewModel;

namespace Voracious.Core.Interface;

public interface IBookNote
{
    int Id { get; set; }

    string BookId { get; set; }

    ObservableCollection<UserNoteViewModel>? Notes { get; set; }
}