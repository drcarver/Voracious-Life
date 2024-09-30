using System.Collections.Generic;
using System.Linq;
using System.Text;

using Voracious.EPub.Format;
using Voracious.EPub.Misc;

namespace Voracious.EPub;

public class EpubBook
{
    const string AuthorsSeparator = ", ";

    /// <summary>
    /// Read-only raw epub format structures.
    /// </summary>
    public EpubFormat Format { get; set; }

    public string Title => Format.Opf.Metadata.Titles.FirstOrDefault();

    public IEnumerable<string> Authors => Format.Opf.Metadata.Creators.Select(creator => creator.Text);

    /// <summary>
    /// All files within the EPUB.
    /// </summary>
    public EpubResources Resources { get; set; }

    /// <summary>
    /// EPUB format specific resources.
    /// </summary>
    public EpubSpecialResources SpecialResources { get; set; }

    public byte[] CoverImage { get; set; }

    public List<EpubChapter> TableOfContents { get; set; }

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
