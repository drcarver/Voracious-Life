using System.IO;
using System.Text;

namespace Voracious.RDF.Extension;

/// <summary>
/// Utility
/// </summary>
public static class Utility
{
    /// <summary>
    /// Fix up a string so that it will be a valid file name. It will be limited to 20 chars,
    /// won't have a ":", "/", "\", NUL, etc. 
    /// See https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file
    /// </summary>
    /// <param name="str">The string to check</param>
    /// <returns>A valid file name</returns>
    public static string AsValidFilename(string str)
    {
        if (str == null) return "_";

        if (string.IsNullOrEmpty(str.Trim()))
            return "_";

        var sb = new StringBuilder();
        foreach (var ch in str.Trim())
        {
            if (ch < 20 || ch == 0x7F || ch == '<' || ch == '>' || ch == ':' || ch == '/'
                || ch == '\\' || ch == '|' || ch == '?' || ch == '*'
                || ch == '\'' || ch == '"')
            {
                sb.Append('_');
            }
            else
            {
                sb.Append(ch);
            }
        }
        var retval = sb.ToString().TrimEnd('.');

        // These are all reserved words. I'm actually grabbing a
        // superset of reserved words technically you can call a
        // file consequence.txt but I disallow it.
        var upper = retval.ToUpper();
        if (upper.StartsWith("CON") || upper.StartsWith("PRN")
            || upper.StartsWith("AUX") || upper.StartsWith("COM")
            || upper.StartsWith("LPT"))
        {
            var suffix = retval.Length == 3 ? "" : retval.Substring(3);
            retval = retval.Substring(0, 3) + "_" + suffix;
        }

        return retval;
    }

    /// <summary>
    /// Convert a stream to a string
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="enc"></param>
    /// <returns></returns>
    public static string ReadAllText(this Stream stream)
    {
        byte[] bytes = new byte[stream.Length];
        stream.Position = 0;
        stream.Read(bytes, 0, (int)stream.Length);
        return Encoding.UTF8.GetString(bytes);
    }
}
