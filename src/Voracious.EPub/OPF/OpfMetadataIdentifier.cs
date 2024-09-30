using System.Xml.Linq;

namespace Voracious.EPub.OPF;

public class OpfMetadataIdentifier
{
    public static class Attributes
    {
        public static readonly XName Id = "id";
        public static readonly XName Scheme = "scheme";
    }

    public string Id { get; set; }
    public string Scheme { get; set; }
    public string Text { get; set; }
}
