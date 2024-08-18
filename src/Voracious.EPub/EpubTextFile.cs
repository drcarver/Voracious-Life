using Voracious.EPub.Format;

namespace Voracious.EPub;

public class EpubTextFile : EpubFile
{
    public string TextContent
    {
        get { return Constants.DefaultEncoding.GetString(Content, 0, Content.Length); }
        set { Content = Constants.DefaultEncoding.GetBytes(value); }
    }
}
