using System.Xml.Linq;

namespace Voracious.EPub.NAV;

public class NavMeta
{
    public static class Attributes
    {
        public static readonly XName Name = "name";
        public static readonly XName Content = "content";
        public static readonly XName Charset = "charset";
    }

    public string Name { get; set; }
    public string Content { get; set; }
    public string Charset { get; set; }
}
