using System.Threading.Tasks;

using Voracious.Control.ViewModel;
using Voracious.EPub;
using Voracious.Life.Model;

namespace Voracious.Life.Interface;

// not a useful warning message.
public interface IBookHandler
{
    EpubFile GetImageByName(string imageName);
    string GetChapterContainingId(string id, int preferredHtmlIndex);
    Task<string> GetChapterBeforePercentAsync(BookLocation location);
    Task DisplayBook(ResourceViewModel book, BookLocation location = null);
    Task SetFontAndSizeAsync(string font, string size); // sie is e.g. "12pt"
}
