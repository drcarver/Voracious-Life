using System.Xml.Linq;

namespace Voracious.RDF.Model;

public class Identifier
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
