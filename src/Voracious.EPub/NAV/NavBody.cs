using System.Collections.Generic;
using System.Xml.Linq;

namespace Voracious.EPub.NAV;

public class NavBody
{
    /// <summary>
    /// Instantiated only when the EPUB was read.
    /// </summary>
    internal XElement Dom { get; set; }

    public IList<NavNav> Navs { get; internal set; } = new List<NavNav>();
}
