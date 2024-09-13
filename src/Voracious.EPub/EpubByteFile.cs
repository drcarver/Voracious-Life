namespace Voracious.EPub;

public class EpubByteFile : EpubFile
{
    public EpubTextFile ToTextFile()
    {
        return new EpubTextFile
        {
            Content = Content,
            ContentType = ContentType,
            AbsolutePath = AbsolutePath,
            Href = Href,
            MimeType = MimeType
        };
    }
}
