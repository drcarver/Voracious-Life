using System.Xml.Linq;

namespace Voracious.EPub.OPF;

public class OpfMetadataMeta
{
    public static class Attributes
    {
        public static readonly XName Id = "id";
        public static readonly XName Name = "name";
        public static readonly XName Refines = "refines";
        public static readonly XName Scheme = "scheme";
        public static readonly XName Property = "property";
        public static readonly XName Content = "content";
    }

    public string Name { get; set; }
    public string Id { get; set; }
    public string Refines { get; set; }
    public string Property { get; set; }
    public string Scheme { get; set; }
    public string Text { get; set; }
}
