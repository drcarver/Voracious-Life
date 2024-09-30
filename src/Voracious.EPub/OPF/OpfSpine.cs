using System.Collections.Generic;
using System.Xml.Linq;

namespace Voracious.EPub.OPF;

public class OpfSpine
{
    public static class Attributes
    {
        public static readonly XName Toc = "toc";
    }

    public string Toc { get; set; }
    public List<OpfSpineItemRef> ItemRefs { get; set; } = new List<OpfSpineItemRef>();
}
