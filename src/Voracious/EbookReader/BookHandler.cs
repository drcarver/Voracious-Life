using Voracious.Core.ViewModel;
using Voracious.EPub;

namespace Voracious.Database;

// not a useful warning message.
#pragma warning disable IDE1006
public interface BookHandler
{
    EpubFile GetImageByName(string imageName);
    string GetChapterContainingId(string id, int preferredHtmlIndex);
    Task<string> GetChapterBeforePercentAsync(BookLocation location);
    Task DisplayBook(ResourceViewModel book, BookLocation location = null);
    Task SetFontAndSizeAsync(string font, string size); // sie is e.g. "12pt"
}
public interface SimpleBookHandler
{
    Task DisplayBook(ResourceViewModel book, BookLocation location);
}
