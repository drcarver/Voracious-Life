using System.Threading.Tasks;

using Voracious.Core.Enum;

namespace Voracious.Core.Interface;

public interface IIndexReaderCore
{
    void BookEnd(BookStatusEnum status, IResource book);
    Task LogAsync(string str);
    void SetFileSize(ulong size);
    Task OnStreamProgressAsync(uint bytesRead);
    Task OnStreamTotalProgressAsync(ulong bytesRead);
    Task OnStreamCompleteAsync();
    Task OnAddNewBook(IResource bookData);
    Task OnTotalBooks(int nbooks); // How many books have been checked (new and old together)
    Task OnReadComplete(int nBooksTotal, int nNewBooks);
}
