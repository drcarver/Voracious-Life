using System.Collections.Generic;
using System.Xml.Linq;

namespace Voracious.EPub.OPF;

public class OpfManifestItem
{
    public static class Attributes
    {
        public static readonly XName Fallback = "fallback";
        public static readonly XName FallbackStyle = "fallback-style";
        public static readonly XName Href = "href";
        public static readonly XName Id = "id";
        public static readonly XName MediaType = "media-type";
        public static readonly XName Properties = "properties";
        public static readonly XName RequiredModules = "required-modules";
        public static readonly XName RequiredNamespace = "required-namespace";
    }

    public string Id { get; set; }
    public string Href { get; set; }
    public List<string> Properties { get; set; } = [];
    public string MediaType { get; set; }
    public string RequiredNamespace { get; set; }
    public string RequiredModules { get; set; }
    public string Fallback { get; set; }
    public string FallbackStyle { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, Href = {Href}, MediaType = {MediaType}";
    }
}
