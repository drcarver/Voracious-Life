using System.Collections.Generic;
using System.Xml.Linq;

namespace Voracious.EPub.NAV;

public class NavBody
{
    /// <summary>
    /// Instantiated only when the EPUB was read.
    /// </summary>
    public XElement Dom { get; set; }

    public List<NavNav> Navs { get; set; } = new List<NavNav>();
}
