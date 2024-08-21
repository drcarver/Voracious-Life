using System.IO;
using System;
using System.Text;

using Voracious.Core.Enum;
using Voracious.Core.ViewModel;
using Voracious.Core.Interface;

namespace Voracious.Core.Extension;

/// <summary>
/// Utility
/// </summary>
public static class Utility
{
    public static int GetImportance(IPerson person)
    {
        switch (person.PersonType)
        {
            case RelatorEnum.author: return 10;
            case RelatorEnum.artist: return 20;
            case RelatorEnum.editor: return 30;
            case RelatorEnum.photographer: return 40;
            case RelatorEnum.translator: return 50;
            case RelatorEnum.illustrator: return 60;

            case RelatorEnum.adapter: return 70;
            case RelatorEnum.annotator: return 80;
            case RelatorEnum.authorOfAfterward: return 90;
            case RelatorEnum.arranger: return 100;
            case RelatorEnum.compiler: return 110;
            case RelatorEnum.composer: return 120;
            case RelatorEnum.conductor: return 130;
            case RelatorEnum.performer: return 140;
            case RelatorEnum.librettist: return 150;

            case RelatorEnum.authorOfIntroduction: return 160;
            case RelatorEnum.collaborator: return 170;
            case RelatorEnum.commentator: return 180;
            case RelatorEnum.contributor: return 190;
            case RelatorEnum.dubiousAuthor: return 200;
            case RelatorEnum.editorOfCompilation: return 210;
            case RelatorEnum.engraver: return 220;
            case RelatorEnum.other: return 230;
            case RelatorEnum.publisher: return 240;
            case RelatorEnum.researcher: return 250;
            case RelatorEnum.transcriber: return 260;
            case RelatorEnum.unknown: return 270;
            case RelatorEnum.otherError: return 280;
        }
        return 999;
    }

    // unknown is e.g. book http://www.gutenberg.org/ebooks/2822
    // where Daniel Defoe is somehow part of this book, we just
    // don't know how.  In the text, the book is attributed to Defoe.
    public static RelatorEnum ToRelator(IPerson person, string value)
    {
        switch (value)
        {
            // https://www.loc.gov/marc/relators/relaterm.html
            case "dcterms:creator": return RelatorEnum.author; // little bit of magic :-)
            case "marcrel:adp": return RelatorEnum.adapter;
            case "marcrel:art": return RelatorEnum.artist;
            case "marcrel:aft": return RelatorEnum.authorOfAfterward;
            case "marcrel:ann": return RelatorEnum.annotator;
            case "marcrel:arr": return RelatorEnum.arranger;
            case "marcrel:aui": return RelatorEnum.authorOfIntroduction;
            case "marcrel:aut": return RelatorEnum.author;
            case "marcrel:clb": return RelatorEnum.collaborator;
            case "marcrel:cmm": return RelatorEnum.commentator;
            case "marcrel:cmp": return RelatorEnum.composer;
            case "marcrel:cnd": return RelatorEnum.conductor;
            case "marcrel:com": return RelatorEnum.compiler;
            case "marcrel:ctb": return RelatorEnum.contributor;
            case "marcrel:dub": return RelatorEnum.dubiousAuthor;
            case "marcrel:edc": return RelatorEnum.editorOfCompilation;
            case "marcrel:edt": return RelatorEnum.editor;
            case "marcrel:egr": return RelatorEnum.engraver;
            case "marcrel:ill": return RelatorEnum.illustrator;
            case "marcrel:lbt": return RelatorEnum.librettist;
            case "marcrel:oth": return RelatorEnum.other;
            case "marcrel:pbl": return RelatorEnum.publisher;
            case "marcrel:pht": return RelatorEnum.photographer;
            case "marcrel:prf": return RelatorEnum.performer;
            case "marcrel:prt": return RelatorEnum.printer;
            case "marcrel:res": return RelatorEnum.researcher;
            case "marcrel:trc": return RelatorEnum.transcriber;
            case "marcrel:trl": return RelatorEnum.translator;
            case "marcrel:unk": return RelatorEnum.unknown;


            default:
                return RelatorEnum.otherError; // Distinguish codes that aren't in this list from the actual "other" category
        }
    }

    /// <summary>
    /// Fix up a string so that it will be a valid file name. It will be limited to 20 chars,
    /// won't have a ":", "/", "\", NUL, etc. 
    /// See https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file
    /// </summary>
    /// <param name="str">THe string to check</param>
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
    public static String ReadAllText(this Stream stream)
    {
        byte[] bytes = new byte[stream.Length];
        stream.Position = 0;
        stream.Read(bytes, 0, (int)stream.Length);
        return Encoding.UTF8.GetString(bytes);
    }
}
