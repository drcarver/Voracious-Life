using Voracious.EPub.Enum;

namespace Voracious.EPub;

public abstract class EpubFile
{
    public string AbsolutePath { get; set; }
    public string Href { get; set; }
    public EpubContentEnum ContentType { get; set; }
    public string MimeType { get; set; }
    public byte[] Content { get; set; }

    public override string ToString()
    {
        return $"{AbsolutePath} via HREF {Href}";
    }
}
