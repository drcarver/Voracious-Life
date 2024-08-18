using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Voracious.Database;

/// <summary>
/// Person can be used for Author, Illustrator, Editor, Translator, etc.
/// </summary>
public class Person : INotifyPropertyChanged, INotifyPropertyChanging
{
    private int id;
    private string name = "Unknown";
    private string aliases = "";
    private Relator personType;
    private int birthDate = -999999;
    private int deathDate = 999999;
    private string webpage = "http://wikipedia.com";

    public Person()
    {

    }
    public Person(string name, Person.Relator personType)
    {
        Name = name;
        PersonType = personType;
    }
    public int Id { get => id; set { if (id != value) { NotifyPropertyChanging(); id = value; NotifyPropertyChanged(); } } }
    public enum Relator
    {
        otherError, adapter, artist, authorOfAfterward, annotator, arranger, author, authorOfIntroduction,
        collaborator, commentator, compiler, composer, conductor, contributor,
        dubiousAuthor,
        editor, editorOfCompilation, engraver,
        illustrator, librettist,
        other,
        performer, photographer, publisher,
        researcher, transcriber, translator, unknown,

        // NEW VALUES AT THE END OF THE LIST!
        printer,
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public event PropertyChangingEventHandler PropertyChanging;
    
    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void NotifyPropertyChanging([CallerMemberName] String propertyName = "")
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }

    // unknown is e.g. book http://www.gutenberg.org/ebooks/2822 where Daniel Defoe is somehow part of this book, we just don't know how.
    // In the text, the book is attributed to Defoe.
    public static Relator ToRelator(string value)
    {
        switch (value)
        {
            // https://www.loc.gov/marc/relators/relaterm.html
            case "dcterms:creator": return Relator.author; // little bit of magic :-)
            case "marcrel:adp": return Relator.adapter;
            case "marcrel:art": return Relator.artist;
            case "marcrel:aft": return Relator.authorOfAfterward;
            case "marcrel:ann": return Relator.annotator;
            case "marcrel:arr": return Relator.arranger;
            case "marcrel:aui": return Relator.authorOfIntroduction;
            case "marcrel:aut": return Relator.author;
            case "marcrel:clb": return Relator.collaborator;
            case "marcrel:cmm": return Relator.commentator;
            case "marcrel:cmp": return Relator.composer;
            case "marcrel:cnd": return Relator.conductor;
            case "marcrel:com": return Relator.compiler;
            case "marcrel:ctb": return Relator.contributor;
            case "marcrel:dub": return Relator.dubiousAuthor;
            case "marcrel:edc": return Relator.editorOfCompilation;
            case "marcrel:edt": return Relator.editor;
            case "marcrel:egr": return Relator.engraver;
            case "marcrel:ill": return Relator.illustrator;
            case "marcrel:lbt": return Relator.librettist;
            case "marcrel:oth": return Relator.other;
            case "marcrel:pbl": return Relator.publisher;
            case "marcrel:pht": return Relator.photographer;
            case "marcrel:prf": return Relator.performer;
            case "marcrel:prt": return Relator.printer;
            case "marcrel:res": return Relator.researcher;
            case "marcrel:trc": return Relator.transcriber;
            case "marcrel:trl": return Relator.translator;
            case "marcrel:unk": return Relator.unknown;


            default:
                return Relator.otherError; // Distinguish codes that aren't in this list from the actual "other" category
        }
    }
    public string Name { get => name; set { if (name != value) { NotifyPropertyChanging(); name = value; NotifyPropertyChanged(); } } }
    /// <summary>
    /// Aliases is stored as a series of + seperated names
    /// So it could be "john jones" or "john jones+samantha sams"
    /// Never fiddle with the value directly! Use the AddAlias to add each alias.
    /// </summary>
    public string Aliases { get => aliases; set { if (aliases != value) { NotifyPropertyChanging(); aliases = value; NotifyPropertyChanged(); } } }
    public void AddAlias(string value)
    {
        if (value.Contains('+')) value = value.Replace('+', '&');
        if (string.IsNullOrEmpty(Aliases)) Aliases = value;
        else Aliases = "+" + value;
    }
    public Relator PersonType { get => personType; set { if (personType != value) { NotifyPropertyChanging(); personType = value; NotifyPropertyChanged(); } } }
    // e.g. aut=author ill=illustator from id.loc.gov/vocabulary/relators.html

    public int GetImportance()
    {
        switch (PersonType)
        {
            case Relator.author: return 10;
            case Relator.artist: return 20;
            case Relator.editor: return 30;
            case Relator.photographer: return 40;
            case Relator.translator: return 50;
            case Relator.illustrator: return 60;

            case Relator.adapter: return 70;
            case Relator.annotator: return 80;
            case Relator.authorOfAfterward: return 90;
            case Relator.arranger: return 100;
            case Relator.compiler: return 110;
            case Relator.composer: return 120;
            case Relator.conductor: return 130;
            case Relator.performer: return 140;
            case Relator.librettist: return 150;

            case Relator.authorOfIntroduction: return 160;
            case Relator.collaborator: return 170;
            case Relator.commentator: return 180;
            case Relator.contributor: return 190;
            case Relator.dubiousAuthor: return 200;
            case Relator.editorOfCompilation: return 210;
            case Relator.engraver: return 220;
            case Relator.other: return 230;
            case Relator.publisher: return 240;
            case Relator.researcher: return 250;
            case Relator.transcriber: return 260;
            case Relator.unknown: return 270;
            case Relator.otherError: return 280;
        }
        return 999;
    }

    /// <summary>
    /// Examples:
    /// by Samantha Jones
    /// by Samantha Jones (dubious) 1777-1810
    /// illustrated by Samantha Jones 
    /// </summary>
    public string Summary
    {
        get
        {
            var dates = "";
            if (BirthDate != -999999 && DeathDate != 999999) dates = $"{BirthDate}—{DeathDate}";
            else if (BirthDate != -999999) dates = $"{BirthDate}—";
            else if (DeathDate != 999999) dates = $"—{DeathDate}";
            string retval;
            switch (PersonType)
            {
                case Relator.adapter: retval = $"adapted by {Name}"; break;
                case Relator.annotator: retval = $"annotated by {Name}"; break;
                case Relator.arranger: retval = $"arranged by {Name}"; break;
                case Relator.artist: retval = $"{Name} (artist)"; break;
                case Relator.author: retval = $"by {Name}"; break;
                case Relator.authorOfAfterward: retval = $"afterword by {Name}"; break;
                case Relator.authorOfIntroduction: retval = $"introduction by {Name}"; break;
                case Relator.collaborator: retval = $"{Name} (collaborator)"; break;
                case Relator.commentator: retval = $"{Name} (commentator)"; break;
                case Relator.compiler: retval = $"compiled by {Name}"; break;
                case Relator.composer: retval = $"composed by {Name}"; break;
                case Relator.conductor: retval = $"conducted by {Name}"; break;
                case Relator.contributor: retval = $"{Name} (contributor)"; break;
                case Relator.dubiousAuthor: retval = $"by {Name} (dubious)"; break;
                case Relator.editor: retval = $"edited by {Name}"; break;
                case Relator.editorOfCompilation: retval = $"edited by {Name} (compilation)"; break;
                case Relator.engraver: retval = $"engraved by {Name}"; break;
                case Relator.illustrator: retval = $"illustrated by {Name}"; break;
                case Relator.librettist: retval = $"{Name} (librettist)"; break;
                case Relator.otherError: retval = $"{Name}"; break;
                case Relator.performer: retval = $"performed by {Name}"; break;
                case Relator.photographer: retval = $"{Name} (photographer)"; break;
                case Relator.publisher: retval = $"published by {Name}"; break;
                case Relator.researcher: retval = $"{Name} (researcher)"; break;
                case Relator.transcriber: retval = $"transcribed by {Name}"; break;
                case Relator.translator: retval = $"translated by {Name}"; break;
                case Relator.unknown: retval = $"{Name}"; break;
                default: retval = $"{Name}"; break;
            }
            if (dates != "") retval = retval + " " + dates;
            if (Aliases != "") retval += $" ({Aliases})";
            return retval;
        }
    }


    public int BirthDate { get => birthDate; set { if (birthDate != value) { NotifyPropertyChanging(); birthDate = value; NotifyPropertyChanged(); } } }
    public int DeathDate { get => deathDate; set { if (deathDate != value) { NotifyPropertyChanging(); deathDate = value; NotifyPropertyChanged(); } } }
    public string Webpage { get => webpage; set { if (webpage != value) { NotifyPropertyChanging(); webpage = value; NotifyPropertyChanged(); } } }
    public Uri WebpageUri
    {
        get
        {

            try
            {
                // Some web pages are malformed e.g. eebooks/3453 and won't be converted into URI.
                // May as well filter these out since they will never work.
                if (Webpage.StartsWith("http"))
                {
                    return new Uri(Webpage);
                }
            }
            catch (Exception)
            {
            }
            return new Uri("http://wikipedia.com");
        }
    }

    public override string ToString()
    {
        return $"{Name} relator {PersonType}={(int)PersonType})";
    }
}
