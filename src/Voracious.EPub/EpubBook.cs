using System.Collections.Generic;
using System.Linq;
using System.Text;

using Voracious.EPub.Format;
using Voracious.EPub.Misc;

namespace Voracious.EPub;

public class EpubBook
{
    internal const string AuthorsSeparator = ", ";

    /// <summary>
    /// Read-only raw epub format structures.
    /// </summary>
    public EpubFormat Format { get; internal set; }

    public string Title => Format.Opf.Metadata.Titles.FirstOrDefault();

    public IEnumerable<string> Authors => Format.Opf.Metadata.Creators.Select(creator => creator.Text);

    /// <summary>
    /// All files within the EPUB.
    /// </summary>
    public EpubResources Resources { get; internal set; }

    /// <summary>
    /// EPUB format specific resources.
    /// </summary>
    public EpubSpecialResources SpecialResources { get; internal set; }

    public byte[] CoverImage { get; internal set; }

    public IList<EpubChapter> TableOfContents { get; internal set; }

    public string ToPlainText()
    {
        var builder = new StringBuilder();
        foreach (var html in SpecialResources.HtmlInReadingOrder)
        {
            builder.Append(Html.GetContentAsPlainText(html.TextContent));
            builder.Append('\n');
        }
        return builder.ToString().Trim();
    }
}
