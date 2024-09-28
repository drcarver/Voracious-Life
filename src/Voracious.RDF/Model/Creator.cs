using System.Collections.Generic;
using System.Xml.Linq;

using Voracious.RDF.Enum;
using Voracious.RDF.Interface;

namespace Voracious.RDF.Model;

public class Creator : ICreator
{
    public static class Attributes
    {
        public static readonly XName Role = Constants.OpfNamespace + "role";
        static readonly XName FileAs = Constants.OpfNamespace + "file-as";
        static readonly XName AlternateScript = Constants.OpfNamespace + "alternate-script";
    }

    /// <summary>
    /// The primary key
    /// </summary>
    public string About { get; set; }

    /// <summary>
    /// The alternate script for the creator
    /// </summary>
    public string? AlternateScript { get; set; } = null;

    /// <summary>
    /// The name of the person
    /// </summary>
    public string? Name { get; set; } = null;

    /// <summary>
    /// The person's alias
    /// </summary>
    public string? Aliases { get; set; } = null;

    /// <summary>
    /// The date of birth
    /// </summary>
    public int? BirthDate { get; set; } = null;

    /// <summary>
    /// The date of death
    /// </summary>
    public int? DeathDate { get; set; } = null;

    /// <summary>
    /// The author's web page
    /// </summary>
    public string? Webpage { get; set; } = null;

    /// <summary>
    /// The sortable version of the author
    /// </summary>
    public string? FileAs { get; set; } = null;

    /// <summary>
    /// The relator's for the book (creator, illustrator, etc..)
    /// </summary>
    public RelatorEnum? Role { get; set; } = null;

    /// <summary>
    /// People include authors, illustrators, etc.
    /// </summary>
    public List<Resource> Resources { get; set; } = [];

    /// <summary>
    /// Add the alias for the person
    /// </summary>
    /// <param name="value"></param>
    public void AddAlias(string value)
    {
        if (value.Contains("+"))
        {
            value = value.Replace('+', '&');
        }
        if (string.IsNullOrEmpty(Aliases))
        {
            Aliases = value;
        }
        else Aliases = "+" + value;
    }

    /// <summary>
    /// Get the Importance of a Creator to the resource
    /// </summary>
    /// <returns>The persons importance</returns>
    public int GetImportance()
    {
        switch (Role)
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

    /// <summary>
    /// unknown is e.g. book http://www.gutenberg.org/ebooks/2822
    /// where Daniel Defoe is somehow part of this book, we just
    /// don't know how.  In the text, the book is attributed to Defoe.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public RelatorEnum ToRelator(string value)
    {
        switch (value)
        {
            // https://www.loc.gov/marc/relators/relaterm.html
            case "dcterms:creator":
                return RelatorEnum.author; // little bit of magic :-)
            case "marcrel:adp":
                return RelatorEnum.adapter;
            case "marcrel:art":
                return RelatorEnum.artist;
            case "marcrel:aft":
                return RelatorEnum.authorOfAfterward;
            case "marcrel:ann":
                return RelatorEnum.annotator;
            case "marcrel:arr":
                return RelatorEnum.arranger;
            case "marcrel:aui":
                return RelatorEnum.authorOfIntroduction;
            case "marcrel:aut":
                return RelatorEnum.author;
            case "marcrel:clb":
                return RelatorEnum.collaborator;
            case "marcrel:cmm":
                return RelatorEnum.commentator;
            case "marcrel:cmp":
                return RelatorEnum.composer;
            case "marcrel:cnd":
                return RelatorEnum.conductor;
            case "marcrel:com":
                return RelatorEnum.compiler;
            case "marcrel:ctb":
                return RelatorEnum.contributor;
            case "marcrel:dub":
                return RelatorEnum.dubiousAuthor;
            case "marcrel:edc":
                return RelatorEnum.editorOfCompilation;
            case "marcrel:edt":
                return RelatorEnum.editor;
            case "marcrel:egr":
                return RelatorEnum.engraver;
            case "marcrel:ill":
                return RelatorEnum.illustrator;
            case "marcrel:lbt":
                return RelatorEnum.librettist;
            case "marcrel:oth":
                return RelatorEnum.other;
            case "marcrel:pbl":
                return RelatorEnum.publisher;
            case "marcrel:pht":
                return RelatorEnum.photographer;
            case "marcrel:prf":
                return RelatorEnum.performer;
            case "marcrel:prt":
                return RelatorEnum.printer;
            case "marcrel:res":
                return RelatorEnum.researcher;
            case "marcrel:trc":
                return RelatorEnum.transcriber;
            case "marcrel:trl":
                return RelatorEnum.translator;
            case "marcrel:unk":
                return RelatorEnum.unknown;


            default:
                return RelatorEnum.otherError; // Distinguish codes that aren't in this list from the actual "other" category
        }
    }
}
