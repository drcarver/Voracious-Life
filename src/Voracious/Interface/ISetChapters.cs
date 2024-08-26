using Voracious.EbookReader;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Voracious.Interface;

public interface ISetChapters
{
    void SetChapters(EpubBookExt book, IList<EpubChapter> chapters);
}
