using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.Core.Interface;

namespace Voracious.Core.ViewModel;

public partial class BookNoteViewModel : ObservableObject, IBookNote
{
    [ObservableProperty]
    [property: Key]
    private int id;

    [ObservableProperty]
    private string bookId;

    [ObservableProperty]
    private ObservableCollection<UserNoteViewModel> notes = [];
}
