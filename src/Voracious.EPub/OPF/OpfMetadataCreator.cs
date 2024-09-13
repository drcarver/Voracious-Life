using System.Xml.Linq;

using Voracious.EPub.Format;

namespace Voracious.EPub.OPF;

public class OpfMetadataCreator
{
    public static class Attributes
    {
        public static readonly XName Role = Constants.OpfNamespace + "role";
        public static readonly XName FileAs = Constants.OpfNamespace + "file-as";
        public static readonly XName AlternateScript = Constants.OpfNamespace + "alternate-script";
    }

    public string Text { get; set; }
    public string Role { get; set; }
    public string FileAs { get; set; }
    public string AlternateScript { get; set; }
}
