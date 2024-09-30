using System.Collections.Generic;
using System.Xml.Linq;

namespace Voracious.EPub.OPF;

public class OpfSpineItemRef
{
    public static class Attributes
    {
        public static readonly XName IdRef = "idref";
        public static readonly XName Linear = "linear";
        public static readonly XName Id = "id";
        public static readonly XName Properties = "properties";
    }

    public string IdRef { get; set; }
    public bool Linear { get; set; }
    public string Id { get; set; }
    public List<string> Properties { get; set; } = new List<string>();

    public override string ToString()
    {
        return "IdRef: " + IdRef;
    }
}
