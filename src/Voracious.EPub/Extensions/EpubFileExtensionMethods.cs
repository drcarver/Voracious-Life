namespace Voracious.EPub.Extensions;

public static class EPubFileExtensionMethods
{
    public static string FileName(this EpubChapter epub)
    {
        var str = epub.AbsolutePath;
        var lastIndex = str.LastIndexOf('/');
        if (lastIndex < 0) 
        {
            return str; 
        }
        return str.Substring(lastIndex + 1);
    }

    public static string FileName(this EpubFile epub)
    {
        var str = epub.AbsolutePath;
        var lastIndex = str.LastIndexOf('/');
        if (lastIndex < 0)
        { 
            return str; 
        }
        return str.Substring(lastIndex + 1);
    }

    public static string FileName(this EpubByteFile epub)
    {
        var str = epub.AbsolutePath;
        var lastIndex = str.LastIndexOf('/');
        if (lastIndex < 0)
        {
            return str;
        }
        return str.Substring(lastIndex + 1);
    }

    public static string FileName(this EpubTextFile epub)
    {
        var str = epub.AbsolutePath;
        var lastIndex = str.LastIndexOf('/');
        if (lastIndex < 0)
        {
            return str;
        }
        return str.Substring(lastIndex + 1);
    }
}
