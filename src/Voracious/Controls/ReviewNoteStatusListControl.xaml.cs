using System.Collections.ObjectModel;

using Voracious.Database;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Voracious.Controls
{
    public sealed partial class ReviewNoteStatusListControl : ContentView
    {
        public ObservableCollection<BookDataViewModel> BookList { get; } = new ObservableCollection<BookDataViewModel>();
        public ReviewNoteStatusListControl()
        {
            this.DataContext = this;
            this.InitializeComponent();
        }
        public void SetBookList(IList<BookDataViewModel> list)
        {
            BookList.Clear();
            foreach (var bookData in list)
            {
                BookList.Add(bookData);
            }
        }
    }
}
