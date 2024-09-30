using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Voracious.RDF.Interface;

/// <summary>
/// Contains all of the common queries in the book databases
/// </summary>
public interface IBookService
{
    int NQueries { get; set; }

    int BookCount();

    IResource? GetBook(string bookId);

    /// <summary>
    /// List of books in the Gutenberg Top 100
    /// </summary>
    string[] Top100Books { get; }

    /// <summary>
    /// Given the InitialBookIds from Gutenberg, download each 
    /// one into the Assets/PreinstalledBooks folder. Does a 
    /// pause between each download to be nice, so it's a slow 
    /// process. 
    /// </summary>
    /// <returns></returns>
    Task<int> DownloadBooksAsync();

    /// <summary>
    /// Returns an abbreviated set of data with just the Files. This is used when merging
    /// a new catalog with an old catalog; the new catalog might have more files than the
    /// old catalog. This is super-common with the latest books which might just be available
    /// as .TXT files at the start.
    /// </summary>
    /// <param name="bookdb"></param>
    /// <param name="bookId"></param>
    /// <returns></returns>
    IResource? GetBookFiles(string bookId);

    ObservableCollection<IResource> GetAllBookWhichHaveUserData();

    ObservableCollection<IResource> GetBookRecentWhichHaveUserData();

    IUserNote? FindBookNote(string bookId);

    ObservableCollection<IUserNote> GetAllBookNotes();

    IUserReview FindUserReview(string bookId);

    ObservableCollection<IUserReview> GetAllUserReviews();
}
