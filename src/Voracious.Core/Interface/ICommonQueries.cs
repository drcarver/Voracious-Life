using System.Collections.Generic;

namespace Voracious.Core.Interface;

/// <summary>
/// Contains all of the common queries in the book databases
/// </summary>
public interface IBookService
{
    int NQueries { get; set; }

    int BookCount();

    IBook? GetBook(string bookId);

    /// <summary>
    /// Returns an abbreviated set of data with just the Files. This is used when merging
    /// a new catalog with an old catalog; the new catalog might have more files than the
    /// old catalog. This is super-common with the latest books which might just be available
    /// as .TXT files at the start.
    /// </summary>
    /// <param name="bookdb"></param>
    /// <param name="bookId"></param>
    /// <returns></returns>
    IBook? GetBookFiles(string bookId);

    List<IBook> GetAllBookWhichHaveUserData();

    List<IBook> GetBookRecentWhichHaveUserData();

    IBookNote? FindBookNote(string bookId);

    List<IBookNote> GetAllBookNotes();

    IDownloadData FindDownloaded(string bookId);

    List<IDownloadData> GetAllDownloadedBooks();

    IUserReview FindUserReview(string bookId);

    List<IUserReview> GetAllUserReviews();
}
