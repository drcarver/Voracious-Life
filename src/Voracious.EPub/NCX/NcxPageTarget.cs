using System.Xml.Linq;

namespace Voracious.EPub.NCX;

public class NcxPageTarget
{
    public static class Attributes
    {
        public static readonly XName Id = "id";
        public static readonly XName Class = "class";
        public static readonly XName Type = "type";
        public static readonly XName Value = "value";
        public static readonly XName ContentSrc = "src";
    }

    public string Id { get; set; }
    public string Value { get; set; }
    public string Class { get; set; }
    public NcxPageTargetType? Type { get; set; }
    public string NavLabelText { get; set; }
    public string ContentSrc { get; set; }
}
