//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//using Newtonsoft.Json;

//using Voracious.Core.Enum;
//using Voracious.Core.ViewModel;

//// See https://github.com/Microsoft/Windows-universal-samples/tree/master/Samples/FileAccess
//// for how we use the future access and most recently used (MRU)
//// MRU: StorageApplicationPermissions.MostRecentlyUsedList
//// Future access list: StorageApplicationPermissions.FutureAccessList
//// 
//namespace Voracious.Core.Model;

///// <summary>
///// Smart class to handle bookmarks. Smarts include keeping track of the file in the 
///// MRU list and the FutureAccess list
///// </summary>
//public partial class BookMarkFile
//{
//    public readonly string SavedFromName = Environment.MachineName;
//    public DateTimeOffset SaveTime { get; set; } = DateTimeOffset.Now;
//    public List<BookDataViewModel> BookMarkList { get; set; }

//    /// <summary>
//    /// Merges the changes from a single read-in bookmark file 
//    /// into the local database.
//    /// </summary>
//    /// <param name="bmf"></param>
//    /// <returns></returns>
//    private async Task<int> MergeAsync(BookMarkFile bmf)
//    {
//        int nchanges = 0;
//        // Now let's be very smart about combining this file in with the original.
//        var bookdb = BookDataContext.Get();
//        var currbooks = CommonQueries.BookGetAllWhichHaveUserData(bookdb);
//        const int CHANGES_PER_SAVE = 1000;
//        int nextDbSaveChange = CHANGES_PER_SAVE;
//        foreach (var external in bmf.BookMarkList)
//        {
//            var book = currbooks.Find(b => b.BookId == external.BookId);
//            if (book == null) book = CommonQueries.BookGet(bookdb, external.BookId);
//            if (book == null)
//            {
//                // Perpend the BookMarkSource so that the book is clearly labeled
//                // as being from a bookmark file (and therefore this is kind of a fake entry)
//                if (!external.BookSource.StartsWith(BookDataViewModel.BookSourceBookMarkFile))
//                {
//                    external.BookSource = BookDataViewModel.BookSourceBookMarkFile + external.BookSource;
//                }
//                // Must set all these ids to zero so that they get re-set by EF.
//                if (external.Review != null) external.Review.Id = 0;
//                if (external.Notes != null)
//                {
//                    external.Notes.Id = 0;
//                    foreach (var note in external.Notes.Notes)
//                    {
//                        note.Id = 0;
//                    }
//                }
//                if (external.NavigationData != null) external.NavigationData.Id = 0;
//                external.DownloadData = null; // on this computer, nothing has been downloaded.

//                CommonQueries.BookAdd(bookdb, external, CommonQueries.ExistHandling.IfNotExists);
//                nchanges++;
//                App.Error($"NOTE: adding external {external.BookId} name {external.Title}");
//            }
//            else
//            {
//                // Great -- now I can merge the UserReview, Notes, and BookNavigationData.
//                int nbookchanges = 0;
//                if (external.Review != null)
//                {
//                    if (book.Review == null)
//                    {
//                        external.Review.Id = 0; // clear it out so that EF will set to the correct value.
//                        book.Review = external.Review;
//                        nbookchanges++;
//                    }
//                    else
//                    {
//                        nbookchanges += book.Review.Merge(external.Review);
//                    }
//                }

//                if (external.NavigationData != null)
//                {
//                    if (book.NavigationData == null)
//                    {
//                        external.NavigationData.Id = 0; // clear it out so that EF will set to the correct value.
//                        book.NavigationData = external.NavigationData;
//                        nbookchanges++;
//                    }
//                    else
//                    {
//                        nbookchanges += book.NavigationData.Merge(external.NavigationData);
//                    }
//                }

//                if (external.Notes != null)
//                {
//                    if (book.Notes == null)
//                    {
//                        // Copy them all over
//                        book.Notes = new BookNoteViewModel()
//                        {
//                            BookId = external.Notes.BookId,
//                        };
//                        foreach (var note in external.Notes.Notes)
//                        {
//                            note.Id = 0; // reset to zero to insert into the currrent book.
//                            book.Notes.Notes.Add(note);
//                        }
//                        nbookchanges++;
//                    }
//                    else
//                    {
//                        // Add in only the changed notes. The ids will not be the same
//                        nbookchanges += book.Notes.Merge(external.Notes);
//                    }
//                }

//                if (nbookchanges > 0)
//                {
//                    ; // hook to hang the debugger on.
//                }
//                nchanges += nbookchanges;

//                if (nchanges > nextDbSaveChange)
//                {
//                    await bookdb.SaveChangesAsync();
//                    nextDbSaveChange = nchanges + CHANGES_PER_SAVE;
//                }
//            }
//        }

//        // And save at the end!
//        if (nchanges > 0)
//        {
//            await bookdb.SaveChangesAsync();
//        }

//        return nchanges;
//    }

//    public async Task SmartSaveAsync(BookMarkFileEnum saveType)
//    {
//        var preferredName = saveType == BookMarkFileEnum.RecentOnly
//            ? Environment.MachineName + ".recent" + BookmarkFileDirectory.EXTENSION
//            : "FullBookmarkFile" + BookmarkFileDirectory.EXTENSION
//            ;
//        StorageFolder folder = null;
//        try
//        {
//            folder = await BookmarkFileDirectory.GetBookmarkFolderAsync();
//            if (folder == null)
//            {
//                folder = await SetSaveFolderAsync();
//            }
//            if (folder == null) return;

//            if (folder != null)
//            {
//                // Make sure the files are in sync first
//                int nread = await SmartReadAsync();
//                System.Diagnostics.Debug.WriteLine($"Smart save: smart read {nread} files");

//                var bmf = CreateBookMarkFile(saveType);
//                var str = bmf.AsFileString();
//                await BookmarkFileDirectory.WriteFileAsync(folder, preferredName, str);
//            }

//        }
//        catch (Exception ex)
//        {
//            App.Error($"Unable to save file {preferredName} to folder {folder} exception {ex.Message}");
//        }
//    }

//    public BookMarkFile CreateBookMarkFile(BookMarkFileEnum fileType)
//    {
//        var retval = new BookMarkFile();
//        var bookdb = BookDataContext.Get();

//        var list = fileType == BookMarkFileEnum.FullFile
//            ? CommonQueries.BookGetAllWhichHaveUserData(bookdb)
//            : CommonQueries.BookGetRecentWhichHaveUserData(bookdb);

//        // We only save some of the BookData fields in a book mark file. 
//        // Don't bother with the full file list (total waste of time), or the people list.
//        var trimmedList = new List<BookDataViewModel>();
//        foreach (var book in list)
//        {
//            trimmedList.Add(CreateBookMarkBookData(book));
//        }

//        retval.BookMarkList = trimmedList;
//        return retval;
//    }

//    /// <summary>
//    /// A full BookData has too much data to write into the BookMark file. Trim it down
//    /// by removing elements that are never changed by the user and aren't needed to
//    /// merge into the other computer's databases
//    /// </summary>
//    /// <param name="source"></param>
//    /// <returns></returns>
//    private static BookDataViewModel CreateBookMarkBookData(BookDataViewModel source)
//    {
//        BookDataViewModel retval = new BookDataViewModel()
//        {
//            BookId = source.BookId,
//            Title = source.Title,
//            BookSource = source.BookSource,
//            BookType = source.BookType,
//            Issued = source.Issued,
//            Language = source.Language,
//            DenormDownloadDate = source.DenormDownloadDate,
//            DenormPrimaryAuthor = source.DenormPrimaryAuthor,

//            // Complex structures to be copied
//            NavigationData = source.NavigationData,
//            Notes = source.Notes,
//            Review = source.Review,
//        };
//        // DownloadData, Files and People are explicity kept blank
//        return retval;
//    }

//    /// <summary>
//    /// return this as a Json String
//    /// </summary>
//    /// <returns></returns>
//    public string AsJsonString()
//    {
//        return JsonConvert.SerializeObject(this, Formatting.Indented);
//    }
//}
