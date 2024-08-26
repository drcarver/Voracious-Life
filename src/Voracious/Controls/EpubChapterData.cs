using Voracious.Core.Model;
using Voracious.EbookReader;
using Voracious.EpubSharp;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Voracious.Controls;

/// <summary>
/// We get in EpubChapter values, but internally we need a little bit more data.
/// Each incoming EpubChapter is converted to an EpubChapterData value.
/// </summary>
public class EpubChapterData
{
    public EpubChapterData(EpubChapter source, int indent)
    {
        Title = source.Title.Trim();
        FileName = source.FileName();
        Anchor = source.HashLocation; // Was renamed from .Anchor; 5cdfc156747a38f26e008950e8a7043b1d45b4c7 2018-04-18
        Indent = indent;
    }

    public string Title { get; set; }
    public string FileName { get; set; }
    public string Anchor { get; set; }
    public int Indent { get; set; }

    public static BookLocation FromChapter(EpubChapter chapter)
    {
        return new BookLocation()
        {
            Location = chapter.HashLocation,  //.Anchor, -- hashlocation in this context is the part of the url after the hash like example.com/page#section31
            HtmlFileName = chapter.FileName(),
        };
    }
    public static BookLocation FromChapter(EpubChapterData chapter)
    {
        return new BookLocation()
        {
            Location = chapter.Anchor,
            HtmlFileName = chapter.FileName,
        };
    }

    public override string ToString()
    {
        return $"{Title} Anchor={Anchor} Indent={Indent} FileName={FileName}";
    }
}
