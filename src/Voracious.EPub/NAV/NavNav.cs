using System.Xml.Linq;
using Voracious.EPub.Format;

namespace Voracious.EPub.NAV;

public class NavNav
{
    public static class Attributes
    {
        public static readonly XName Id = "id";
        public static readonly XName Class = "class";
        public static readonly XName Type = Constants.OpsNamespace + "type";
        public static readonly XName Hidden = Constants.OpsNamespace + "hidden";

        public static class TypeValues
        {
            public const string Toc = "toc";
            public const string Landmarks = "landmarks";
            public const string PageList = "page-list";
        }
    }

    /// <summary>
    /// Instantiated only when the EPUB was read.
    /// </summary>
    public XElement Dom { get; set; }

    public string Type { get; set; }
    public string Id { get; set; }
    public string Class { get; set; }
    public string Hidden { get; set; }
}
