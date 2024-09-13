using System.Xml.Linq;

namespace Voracious.EPub.OCF;

public class OcfRootFile
{
    public static class Attributes
    {
        public static readonly XName FullPath = "full-path";
        public static readonly XName MediaType = "media-type";
    }

    public string FullPath { get; set; }
    public string MediaType { get; set; }
}
