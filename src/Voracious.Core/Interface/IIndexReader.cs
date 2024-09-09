using System.Threading.Tasks;

using Voracious.Core.Enum;
using Voracious.Core.ViewModel;

namespace Voracious.Core.Interface;

public interface IIndexReader
{
    void BookEnd(BookStatusEnum status, ResourceViewModel book);
    Task LogAsync(string str);
    void SetFileSize(ulong size);
    Task OnStreamProgressAsync(uint bytesRead);
    Task OnStreamTotalProgressAsync(ulong bytesRead);
    Task OnStreamCompleteAsync();
    Task OnAddNewBook(ResourceViewModel bookData);
    Task OnTotalBooks(int nbooks); // How many books have been checked (new and old together)
    Task OnReadComplete(int nBooksTotal, int nNewBooks);
}
