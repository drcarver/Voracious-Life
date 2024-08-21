using System.Text;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using Voracious.Core.Enum;
using Voracious.Core.Interface;
using Voracious.Database.Model;

namespace Voracious.Database;

/// <summary>
/// Contains all of the common queries in the book databases
/// </summary>
partial class CommonQueries
{
    private int _NQueries = 0;
    
    public int NQueries
    {
        get { return _NQueries; }
        set
        {
            _NQueries = value;
            if (_NQueries % 1000 == 0 && _NQueries > 0)
            {
                ; // Spot to break on; we shouldn't have so many queries during normal operation.
                // It's a database; the goal is 1 big query, not a bazillion small ones.
            }
        }
    }

    public enum ExistHandling { IfNotExists, SmartCatalogOverride, CatalogOverrideFast }

    /// <summary>
    /// Adds the bookData into the Books database, but only if it's not already present.
    /// If it's already 
    /// </summary>
    /// <param name="bookData"></param>
    /// <returns>0=not added, 1=added. Technical is the count of the number added.</returns>
    public int BookAdd(VoraciousDataContext bookdb, BookModel book, ExistHandling handling)
    {
        int retval = 0;
        NQueries++;
        lock (bookdb)
        {
            switch (handling)
            {
                case ExistHandling.IfNotExists:
                    if (bookdb.Books.Find(book.BookId) == null)
                    {
                        bookdb.Books.Add(book);
                        retval++;
                    }
                    break;
                case ExistHandling.CatalogOverrideFast:
                    {
                        var dbbook = bookdb.Books.Find(book.BookId);
                        if (dbbook == null)
                        {
                            bookdb.Books.Add(book);
                            retval++;
                        }
                        else // have to be smart.
                        {
                            if (dbbook.BookSource.StartsWith(BookSourceBookMarkFile))
                            {
                                // The database was added to from a bookmark file.
                                // For these books, the dbbook top-level data isn't correct but the user data is correct.
                                // At the same time, the new book top-level data IS correct, but the user data is not correct.
                                BookModel.Merge(dbbook, book);
                                retval++;
                            }
                        }
                    }
                    break;
                case ExistHandling.SmartCatalogOverride:
                    {
                        var dbbook = bookdb.Books.Find(book.BookId);
                        if (dbbook == null)
                        {
                            bookdb.Books.Add(book);
                            retval++;
                        }
                        else // have to be smart.
                        {
                            if (dbbook.BookSource.StartsWith(BookModel.BookSourceBookMarkFile))
                            {
                                // The database was added to from a bookmark file.
                                // For these books, the dbbook top-level data isn't correct but the user data is correct.
                                // At the same time, the new book top-level data IS correct, but the user data is not correct.
                                BookModel.Merge(dbbook, book);
                                retval++;
                            }
                            else
                            {
                                // Grab the full data including the number of files
                                dbbook = BookGetFiles(bookdb, book.BookId);

                                // Ignore everything we just did :-)
                                var mustReplace = !BookModel.FilesMatchEpub(book, dbbook);
                                if (mustReplace)
                                {
                                    //FAIL: project gutenberg LOVES changing their URLs. If the old list doesn't match the 
                                    // new list in number of files, then dump ALL the old values and replace them with the
                                    // new ones.
                                    // TODO: actually verify that the files match?
                                    // Can't use clear because it doesn't work: dbbook.Files.Clear();
                                    // (Seriously: it doesn't work because Files doesn't implement it and will throw)
                                    for (int i = dbbook.Files.Count - 1; i >= 0; i--)
                                    {
                                        dbbook.Files.RemoveAt(i);
                                    }
                                    foreach (var file in book.Files)
                                    {
                                        if (file.Id != 0) file.Id = 0; // if it's straight from the catalog, it should have no id 
                                        dbbook.Files.Add(file);
                                    }
                                    retval++;
                                }
                            }
                        }
                    }
                    break;
            }
            return retval;
        }
    }

    public int BookCount(VoraciousDataContext bookdb)
    {
        NQueries++;
        lock (bookdb)
        {
            var retval = bookdb.Books.Count();
            return retval;
        }
    }

    public BookModel BookGet(VoraciousDataContext bookdb, string bookId)
    {
        NQueries++;
        lock (bookdb)
        {
            var booklist = bookdb.Books
            .Where(b => b.BookId == bookId)
            .Include(b => b.People)
            .Include(b => b.Files)
            .Include(b => b.Review)
            .Include(b => b.Notes)
            .Include(b => b.Notes.Notes)
            .Include(b => b.DownloadData)
            .Include(b => b.NavigationData)
            .AsQueryable();
            ;
            var book = booklist.Where(b => b.BookId == bookId).FirstOrDefault();
            if (book != null && book.BookId == "ebooks/57")
            {
                ; // A good place to hang a debugger on.
            }
            return book;
        }
    }

    /// <summary>
    /// Returns an abbreviated set of data with just the Files. This is used when merging
    /// a new catalog with an old catalog; the new catalog might have more files than the
    /// old catalog. This is super-common with the latest books which might just be available
    /// as .TXT files at the start.
    /// </summary>
    /// <param name="bookdb"></param>
    /// <param name="bookId"></param>
    /// <returns></returns>
    public BookModel BookGetFiles(VoraciousDataContext bookdb, string bookId)
    {
        NQueries++;
        lock (bookdb)
        {
                IQueryable<BookModel> booklist = 
                    bookdb.Books
                        .Where(b => b.BookId == bookId)
                        .Include(b => b.Files)
                        .AsQueryable();

            var book = booklist.Where(b => b.BookId == bookId).FirstOrDefault();
            if (book != null && book.BookId.Contains("62548"))
            {
                ; // A good place to hang a debugger on.
            }
            return book;
        }
    }

    public List<BookModel> BookGetAllWhichHaveUserData(VoraciousDataContext bookdb)
    {
        NQueries++;
        lock (bookdb)
        {
            var booklist = bookdb.Books
            .Include(b => b.Review)
            .Include(b => b.Notes)
            .Include(b => b.Notes.Notes)
            .Include(b => b.DownloadData)
            .Include(b => b.NavigationData)
            .Where(b => b.Review != null || b.Notes != null || b.NavigationData != null)
            .ToList();
            ;
            return booklist;
        }
    }

    public TimeSpan LengthForRecentChanges()
    {
        var recentTimeSpan = new TimeSpan(45, 0, 0, 0); // 45 days
        //var recentTimeSpan = new TimeSpan(0, 1, 0, 0); // For debugging: a paltry 1 hour -- used for debugging
        return recentTimeSpan;
    }

    public List<BookModel> BookGetRecentWhichHaveUserData(VoraciousDataContext bookdb)
    {
        NQueries++;
        var now = DateTimeOffset.Now;
        var recentTimeSpan = LengthForRecentChanges();
        lock (bookdb)
        {
            var booklist = bookdb.Books
            .Include(b => b.Review)
            .Include(b => b.Notes)
            .Include(b => b.Notes.Notes)
            .Include(b => b.DownloadData)
            .Include(b => b.NavigationData)
            .Where(b => b.Review != null || b.Notes != null || b.NavigationData != null)
            .ToList()
            .Where(b => now.Subtract(b.NavigationData.MostRecentNavigationDate) < recentTimeSpan)
            .ToList()
            ;
            return booklist;
        }
    }

    public Task FirstSearchToWarmUpDatabase()
    {
        Task mytask = Task.Run(() =>
        {
            NQueries++;
            DoCreateIndexFile();
        });
        return mytask;
    }

    public void BookDoMigrate(VoraciousDataContext bookdb)
    {
        NQueries++;
    }

    public void BookRemoveAll(VoraciousDataContext bookdb)
    {
        NQueries++;
        lock (bookdb)
        {
            foreach (var book in bookdb.Books)
            {
                bookdb.Books.Remove(book);
            }
        }
    }

    public void BookSaveChanges(VoraciousDataContext bookdb)
    {
        NQueries++;
        lock (bookdb)
        {
            bookdb.SaveChanges();
        }
    }

    public int BookNavigationDataAdd(VoraciousDataContext bookdb, BookNavigationViewModel bn, ExistHandling handling)
    {
        int retval = 0;
        NQueries++;
        var book = BookGet(bookdb, bn.BookId);
        if (book == null) return retval;
        switch (handling)
        {
            case ExistHandling.IfNotExists:
                if (book.NavigationData == null)
                {
                    book.NavigationData = bn;
                    retval++;
                }
                break;
        }
        return retval;
    }

    public BookNavigationModel BookNavigationDataEnsure(VoraciousDataContext bookdb, BookModel bookData)
    {
        var nd = BookNavigationDataFind(bookdb, bookData.BookId);
        if (nd == null)
        {
            nd = new BookNavigationModel()
            {
                BookId = bookData.BookId,
            };
            BookNavigationDataAdd(bookdb, nd, ExistHandling.IfNotExists);
            nd = BookNavigationDataFind(bookdb, bookData.BookId);
            BookSaveChanges(bookdb);
        }
        if (nd == null)
        {
            App.Error($"ERROR: trying to ensure navigation data, but don't have one.");
        }
        return nd;
    }

    public BookNavigationModel BookNavigationDataFind(VoraciousDataContext bookdb, string bookId)
    {
        NQueries++;
        var book = BookGet(bookdb, bookId);
        if (book == null)
        {
            App.Error($"ERROR: attempting to get navigation data for a book={bookId} that doesn't exist");
            return null;
        }
        var retval = book.NavigationData;
        return retval;
    }

    public int BookNotesAdd(VoraciousDataContext bookdb, BookNotes bn, ExistHandling handling)
    {
        int retval = 0;
        NQueries++;
        var book = BookGet(bookdb, bn.BookId);
        if (book == null) return retval;
        switch (handling)
        {
            case ExistHandling.IfNotExists:
                if (book.Notes == null)
                {
                    book.Notes = bn;
                    retval++;
                }
                break;
        }
        return retval;
    }

    public BookNoteModel BookNotesFind(VoraciousDataContext bookdb, string bookId)
    {
        NQueries++;
        var book = BookGet(bookdb, bookId);
        var retval = book.Notes;
        return retval;
    }

    public void BookNoteSave(VoraciousDataContext bookdb, UserNoteViewModel note)
    {
        var bn = BookNotesFind(bookdb, note.BookId);
        if (bn == null)
        {
            bn = new BookNoteViewModel();
            bn.BookId = note.BookId;
            BookNotesAdd(bookdb, bn, ExistHandling.IfNotExists);
            bn = BookNotesFind(bookdb, note.BookId);
        }
        if (note.Id == 0) // Hasn't been saved before. The id is 0.
        {
            bn.Notes.Add(note);
        }
        BookSaveChanges(bookdb);
    }

    public IList<BookNoteModel> BookNotesGetAll()
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

    public int DownloadedBookAdd(VoraciousDataContext bookdb, IDownloadData dd, ExistHandling handling)
    {
        int retval = 0;
        NQueries++;
        var book = BookGet(bookdb, dd.BookId);
        if (book == null) return retval;
        switch (handling)
        {
            case ExistHandling.IfNotExists:
                if (book.DownloadData == null)
                {
                    book.DownloadData = dd;
                    retval++;
                }
                break;
        }
        return retval;
    }

    public void DownloadedBookEnsureFileMarkedAsDownloaded(VoraciousDataContext bookdb, string bookId, string folderPath, string filename)
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

    public DownloadDataModel DownloadedBookFind(VoraciousDataContext bookdb, string bookId)
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

    public List<DownloadDataModel> DownloadedBooksGetAll(VoraciousDataContext bookdb)
    {
        NQueries++;
        lock (bookdb)
        {
            var bookquery = from b in bookdb.Books where b.DownloadData != null select b.DownloadData;
            var retval = bookquery.ToList();
            return retval;
        }
    }

    public int UserReviewAdd(VoraciousDataContext bookdb, UserReviewModel review, ExistHandling handling)
    {
        int retval = 0;
        NQueries++;
        var book = BookGet(bookdb, review.BookId);
        if (book == null) return retval;
        switch (handling)
        {
            case ExistHandling.IfNotExists:
                if (book.Review == null)
                {
                    book.Review = review;
                    retval++;
                }
                break;
        }
        return retval;
    }

    public IUserReview UserReviewFind(VoraciousDataContext bookdb, string bookId)
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

    public List<UserReviewViewModel> UserReviewsGetAll(VoraciousDataContext bookdb)
    {
        NQueries++;
        lock (bookdb)
        {
            var bookquery = from b in bookdb.Books where b.Review != null select b.Review; // NOTE: update all queries to use the dotted format with includes
            var retval = bookquery.ToList();
            return retval;
        }
    }

    public Dictionary<string, BookIndex> BookIndexes = null;

    public void DoCreateIndexFile()
    {
        if (BookIndexes != null) return;
        BookIndexes = new Dictionary<string, BookIndex>();

        var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
        var path = folder.Path;
        string dbpath = Path.Combine(path, VoraciousDataContext.BookDataDatabaseFilename);

        var startTime = DateTime.UtcNow;
        using (var connection = new SqliteConnection($"Data Source={dbpath}"))
        {
            connection.Open();
            AddFromTable(connection, true, "SELECT BookId,Title,TitleAlternative,LCSH,LCCN,LCC,BookSeries FROM Books");
            AddFromTable(connection, false, "SELECT BookDataBookId,Name,Aliases FROM Person");
            AddFromTable(connection, false, "SELECT BookId,Text,Tags FROM UserNote");
            AddFromTable(connection, false, "SELECT BookId,Review,Tags FROM UserReview");
        }
        var delta = DateTime.UtcNow.Subtract(startTime).TotalSeconds;
        Logger.Log($"Time to read index: {delta} seconds");
        ;
    }

    private void AddFromTable(SqliteConnection connection, bool create, string commandText)
    {
        var command = connection.CreateCommand();
        command.CommandText = commandText;

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var bookId = reader.GetString(0);
                var sb = new StringBuilder();
                for (int i = 1; i < reader.FieldCount; i++)
                {
                    if (!reader.IsDBNull(i))
                    {
                        BookIndex.Append(sb, reader.GetString(i));
                    }
                }
                if (create)
                {
                    var index = new BookIndex() { BookId = bookId, Text = sb.ToString() };
                    BookIndexes.Add(index.BookId, index);
                }
                else
                {
                    try
                    {
                        var index = BookIndexes[bookId];
                        index.Text += sb.ToString();
                    }
                    catch (Exception)
                    {
                        ; // Error; why doesn't the book exist?
                    }
                }
            }
        }
    }

    public void DoCreateIndexFileEF()
    {
        if (BookIndexes != null) return;
        BookIndexes = new Dictionary<string, BookIndex>();

        var bookdb = BookDataContext.Get();
        var bookList = bookdb.Books
         .Include(b => b.People)
         .Include(b => b.Review)
         .Include(b => b.Notes)
         .Include(b => b.Notes.Notes)
         .ToList();
        //var sb = new StringBuilder();
        foreach (var bookData in bookList)
        {
            var index = BookIndex.FromBookData(bookData);
            BookIndexes.Add(index.BookId, index);
            //sb.Append(index.ToString());
            //sb.Append('\n');
        }
        //var fullIndex = sb.ToString();
        ;
    }
 
    public HashSet<string> BookSearchs(ISearch searchOperations)
    {
        DoCreateIndexFile(); // create the index as needed.
        var retval = new HashSet<string>();
        foreach (var (bookid, index) in BookIndexes)
        {
            var hasSearch = searchOperations.MatchesFlat(index.Text);
            if (hasSearch)
            {
                retval.Add(index.BookId);
            }
        }
        return retval;
    }
}
