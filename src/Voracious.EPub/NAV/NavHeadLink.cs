using System.Xml.Linq;

namespace Voracious.EPub.NAV;

public class NavHeadLink
{
    public static class Attributes
    {
        public static readonly XName Href = "href";
        public static readonly XName Rel = "rel";
        public static readonly XName Type = "type";
        public static readonly XName Class = "class";
        public static readonly XName Title = "title";
        public static readonly XName Media = "media";
    }

    public string Href { get; set; }
    public string Rel { get; set; }
    public string Type { get; set; }
    public string Class { get; set; }
    public string Title { get; set; }
    public string Media { get; set; }
}
