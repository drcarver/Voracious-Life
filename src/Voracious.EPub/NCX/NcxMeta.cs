using System.Xml.Linq;

namespace Voracious.EPub.NCX;

public class NcxMeta
{
    public static class Attributes
    {
        public static readonly XName Name = "name";
        public static readonly XName Content = "content";
        public static readonly XName Scheme = "scheme";
    }

    public string Name { get; set; }
    public string Content { get; set; }
    public string Scheme { get; set; }
}
