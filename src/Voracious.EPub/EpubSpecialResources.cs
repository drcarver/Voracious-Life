using System.Collections.Generic;

namespace Voracious.EPub;

public class EpubSpecialResources
{
    public EpubTextFile Ocf { get; set; }
    public EpubTextFile Opf { get; set; }
    public List<EpubTextFile> HtmlInReadingOrder { get; set; } = new List<EpubTextFile>();
}
