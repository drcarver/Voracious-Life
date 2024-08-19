using Voracious.Database;
using Voracious.Enum;

namespace Voracious.Controls;

public interface IndexReader
{
    void BookEnd(BookStatusEnum status, BookDataViewModel book);
    Task LogAsync(string str);
    void SetFileSize(ulong size);
    Task OnStreamProgressAsync(uint bytesRead);
    Task OnStreamTotalProgressAsync(ulong bytesRead);
    Task OnStreamCompleteAsync();
    CoreDispatcher GetDispatcher();
    Task OnAddNewBook(BookDataViewModel bookData);
    Task OnTotalBooks(int nbooks); // How many books have been checked (new and old together)
    Task OnReadComplete(int nBooksTotal, int nNewBooks);
}
