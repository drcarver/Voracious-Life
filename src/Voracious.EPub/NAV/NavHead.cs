using System.Collections.Generic;
using System.Xml.Linq;

namespace Voracious.EPub.NAV;

public class NavHead
{
    /// <summary>
    /// Instantiated only when the EPUB was read.
    /// </summary>
    public XElement Dom { get; set; }

    public string Title { get; set; }
    public List<NavHeadLink> Links { get; set; } = new List<NavHeadLink>();
    public List<NavMeta> Metas { get; set; } = new List<NavMeta>();
}
