using System.Xml.Linq;

namespace Voracious.EPub.OPF;

public class OpfGuideReference
{
    public static class Attributes
    {
        public static readonly XName Title = "title";
        public static readonly XName Type = "type";
        public static readonly XName Href = "href";
    }

    public string Type { get; set; }
    public string Title { get; set; }
    public string Href { get; set; }

    public override string ToString()
    {
        return $"Type: {Type}, Href: {Href}";
    }
}
