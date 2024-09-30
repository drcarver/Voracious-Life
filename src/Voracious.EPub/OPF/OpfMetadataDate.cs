using System.Xml.Linq;

using Voracious.EPub.Format;

namespace Voracious.EPub.OPF;

public class OpfMetadataDate
{
    public static class Attributes
    {
        public static readonly XName Event = Constants.OpfNamespace + "event";
    }

    public string Text { get; set; }

    /// <summary>
    /// i.e. "modification"
    /// </summary>
    public string Event { get; set; }
}
