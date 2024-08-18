using System.Xml.Linq;

using Voracious.EPub.Format;

namespace Voracious.EPub.OPF;

public class OpfMetadataDate
{
    internal static class Attributes
    {
        public static readonly XName Event = Constants.OpfNamespace + "event";
    }

    public string Text { get; internal set; }

    /// <summary>
    /// i.e. "modification"
    /// </summary>
    public string Event { get; internal set; }
}
