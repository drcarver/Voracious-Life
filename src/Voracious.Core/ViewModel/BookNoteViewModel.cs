using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.Core.Interface;

namespace Voracious.Core.ViewModel;

public partial class BookNoteViewModel : ObservableObject, IBookNote
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private string bookId;

    [ObservableProperty]
    private ObservableCollection<IUserNote> notes = [];
}
