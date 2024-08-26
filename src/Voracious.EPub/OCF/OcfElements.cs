using System.Xml.Linq;
using Voracious.EPub.Format;

namespace Voracious.EPub.OCF;

internal static class OcfElements
{
    public static readonly XName Container = Constants.OcfNamespace + "container";
    public static readonly XName RootFiles = Constants.OcfNamespace + "rootfiles";
    public static readonly XName RootFile = Constants.OcfNamespace + "rootfile";
}
