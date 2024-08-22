using System.Text;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Voracious.Core.Enum;
using Voracious.Core.Interface;

namespace Voracious.Database;

/// <summary>
/// Contains all of the common queries in the book databases
/// </summary>
partial class BookService : IBookService
{
    public BookService(
        ILoggerFactory loggerFactory,
        VoraciousDataContext db) 
    {
        logger = loggerFactory.CreateLogger<BookService>();
        bookdb = db;
    }

    /// <summary>
    /// The logger
    /// </summary>
    private ILogger logger { get; }

    /// <summary>
    /// The data base context
    /// </summary>
    private VoraciousDataContext bookdb;

    /// <summary>
    /// The number of queries performed
    /// </summary>
    public int NQueries {  get; set; }

    /// <summary>
    /// The number of books in the catalog
    /// </summary>
    /// <returns>The number of books in the catalog</returns>
    public int BookCount()
    {
        NQueries++;
        return bookdb.Books.Count();
    }

    /// <summary>
    /// Get a book by it's id
    /// </summary>
    /// <param name="bookId">The book Id</param>
    /// <returns>The book model</returns>
    public IBook? GetBook(string bookId)
    {
        NQueries++;

        var booklist = bookdb.Books
            .Where(b => b.BookId == bookId)
            .Include(b => b.People)
            .Include(b => b.Files)
            .Include(b => b.Review)
            .Include(b => b.Notes)
            .Include(b => b.Notes.Notes)
            .Include(b => b.DownloadData)
            .Include(b => b.NavigationData);

        return booklist.FirstOrDefault(b => b.BookId == bookId);
    }

    /// <summary>
    /// Returns an abbreviated set of data with just the Files. This is used when merging
    /// a new catalog with an old catalog; the new catalog might have more files than the
    /// old catalog. This is super-common with the latest books which might just be available
    /// as .TXT files at the start.
    /// </summary>
    /// <param name="bookId">The id of the book</param>
    /// <returns>The book with the requested id</returns>
    public IBook? GetBookFiles(string bookId)
    {
        NQueries++;

        IQueryable<IBook> booklist = bookdb.Books
            .Where(b => b.BookId == bookId)
            .Include(b => b.Files)
            .AsQueryable();

        return booklist.FirstOrDefault(b => b.BookId == bookId);
    }

    /// <summary>
    /// Get all books that have user data
    /// </summary>
    /// <returns>All books with the user data</returns>
    public List<IBook> GetAllBookWhichHaveUserData()
    {
        NQueries++;

        return bookdb.Books
            .Include(b => b.Review)
            .Include(b => b.Notes)
            .Include(b => b.Notes.Notes)
            .Include(b => b.DownloadData)
            .Include(b => b.NavigationData)
            .Where(b => b.Review != null || b.Notes != null || b.NavigationData != null)
            .Cast<IBook>()
            .ToList();

        }

    /// <summary>
    /// Remove the most recent books with user data
    /// </summary>
    /// <returns>THe list of most recent books (last 45 days) with user data</returns>
    public List<IBook> GetBookRecentWhichHaveUserData()
    {
        NQueries++;

        return bookdb.Books
            .Include(b => b.Review)
            .Include(b => b.Notes)
            .Include(b => b.Notes.Notes)
            .Include(b => b.DownloadData)
            .Include(b => b.NavigationData)
            .Where(b => b.Review != null 
                || b.Notes != null 
                || b.NavigationData != null)
            .Cast<IBook>()
            .Where(b => b.NavigationData.MostRecentNavigationDate > DateTime.Now.AddDays(-45)) 
            .ToList();
    }

    public IBookNavigation FindBookNavigationData(string bookId)
    {
        NQueries++;
        var book = GetBook(bookId);
        if (book == null)
        {
            logger.LogError($"ERROR: attempting to get navigation data for a book={bookId} that doesn't exist");
            return null;
        }
        return book.NavigationData;
    }

    public IBookNote? FindBookNote(string bookId)
    {
        NQueries++;
        var book = GetBook(bookId);
        return book?.Notes;
    }

    public List<IBookNote> GetAllBookNotes()
    {
        NQueries++;
        var bookdb = BookDataContext.Get();

        var retval = bookdb.Books
            .Include(b => b.Notes)
            .Where(b => b.Notes != null)
            .Include(b => b.Notes.Notes)
            .Select(b => b.Notes)
            .ToList();
        return retval;
    }

    public void DownloadedBookEnsureFileMarkedAsDownloaded(string bookId, string folderPath, string filename)
    {
        NQueries++;
        var book = BookGet(bookdb, bookId);
        if (book == null)
        {
            App.Error($"ERROR: trying to ensure that {bookId} is downloaded, but it's not a valid book");
            return;
        }
        var dd = book.DownloadData;
        if (dd == null)
        {
            dd = new DownloadDataModel()
            {
                BookId = bookId,
                FilePath = folderPath,
                FileName = filename,
                CurrFileStatus = FileStatusEnum.Downloaded,
                DownloadDate = DateTimeOffset.Now,
            };
            book.DenormDownloadDate = dd.DownloadDate.ToUnixTimeSeconds();
            DownloadedBookAdd(bookdb, dd, ExistHandling.IfNotExists);
            BookSaveChanges(bookdb);
        }
        else if (dd.CurrFileStatus != FileStatusEnum.Downloaded)
        {
            dd.FilePath = folderPath;
                dd.CurrFileStatus = FileStatusEnum.Downloaded;
                BookSaveChanges(bookdb);
        }
    }

    public IDownloadData FindDownloaded(string bookId)
    {
        NQueries++;
        var book = BookGet(bookdb, bookId);
        if (book == null)
        {
            App.Error($"ERROR: attempting to get download data for {bookId} that isn't in the database");
            return null;
        }
        var retval = book.DownloadData;
        return retval;
    }

    public List<IDownloadData> GetAllDownloadedBooks()
    {
        NQueries++;
        lock (bookdb)
        {
            var bookquery = from b in bookdb.Books where b.DownloadData != null select b.DownloadData;
            var retval = bookquery.ToList();
            return retval;
        }
    }

    public IUserReview FindUserReview(string bookId)
    {
        NQueries++;
        var book = BookGet(bookdb, bookId);
        if (book == null)
        {
            App.Error($"ERROR: attempting to get user review for {bookId} that isn't in the database");
            return null;
        }
        return book.Review;
    }

    public List<IUserReview> GetAllUserReviews()
    {
        NQueries++;
        lock (bookdb)
        {
            var bookquery = from b in bookdb.Books where b.Review != null select b.Review; // NOTE: update all queries to use the dotted format with includes
            var retval = bookquery.ToList();
            return retval;
        }
    }
}
