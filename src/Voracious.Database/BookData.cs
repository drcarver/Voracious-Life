using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

using Voracious.Database.Interface;

namespace Voracious.Database;

/// <summary>
/// One Gutenberg record for a book (not all data is saved)
/// </summary>
public class BookData : IGetSearchArea, INotifyPropertyChanged, INotifyPropertyChanging
{
    /// <summary>
    /// Return either "" (is valid) or a loggable string of why
    /// the book has problems.
    /// </summary>
    /// <returns></returns>
    public string Validate()
    {
        var retval = "";
        if (string.IsNullOrWhiteSpace(BookId)) retval += "ERROR: BookId is not set\n";
        if (string.IsNullOrWhiteSpace(Title)) retval += "ERROR: Title is not set\n";
        if (!string.IsNullOrWhiteSpace(TitleAlternative) && string.IsNullOrWhiteSpace(Title)) retval += "ERROR: TitleAlternative is set but Title is not\n";

        if (Issued == "None") retval += "ERROR: Book was not issued\n";
        if (Title == "No title" && Files.Count == 0) retval += "ERROR: Gutenberg made a book with no title or files";
        if (retval != "" && Files.Count == 0) retval += "ERROR: Book has no files";
        return retval;
    }

    [System.ComponentModel.DataAnnotations.Key]
    public string BookId { get => bookId; set { if (bookId != value) { NotifyPropertyChanging(); bookId = value; NotifyPropertyChanged(); } } }

    public const string BookSourceGutenberg = "gutenberg.org";
    public const string BookSourceUser = "User-imported";
    public const string BookSourceBookMarkFile = "From-bookmark-file:";
    public string BookSource { get => bookSource; set { if (bookSource != value) { NotifyPropertyChanging(); bookSource = value; NotifyPropertyChanged(); } } }

    public enum FileType { other, Text, Collection, Dataset, Image, MovingImage, Sound, StillImage }; // most are Text. Human genome project e.g 3501 is Dataset.
    public FileType BookType { get => bookType; set { if (bookType != value) { NotifyPropertyChanging(); bookType = value; NotifyPropertyChanged(); } } }
    /// <summary>
    /// Examples:
    /// <dcterms:description>There is an improved edition of this title, eBook #29888</dcterms:description>
    /// <dcterms:description>Illustrated by the author.</dcterms:description>
    /// </summary>
    public string Description { get => description; set { if (description != value) { NotifyPropertyChanging(); description = value; NotifyPropertyChanged(); } } }

    /// <summary>
    /// Examples:
    /// #28: <pgterms:marc260>Houston: Advantage International, The PaperLess Readers Club, 1992</pgterms:marc260>
    /// </summary>
    public string Imprint { get => imprint; set { if (imprint != value) { NotifyPropertyChanging(); imprint = value; NotifyPropertyChanged(); } } }

    public string Issued { get => issued; set { if (issued != value) { NotifyPropertyChanging(); issued = value; NotifyPropertyChanged(); } } }
    /// <summary>
    /// <dcterms:title>Three Little Kittens</dcterms:title>
    /// </summary>
    public string Title { get => title; set { if (title != value) { NotifyPropertyChanging(); title = value; NotifyPropertyChanged(); } } }

    /// <summary>
    /// Used when there is already a title
    /// <dcterms:alternative>Alice in Wonderland</dcterms:alternative>
    /// </summary>
    public string TitleAlternative { get => titleAlternative; set { if (titleAlternative != value) { NotifyPropertyChanging(); titleAlternative = value; NotifyPropertyChanged(); } } }

    /// <summary>
    /// People include authors, illustrators, etc.
    /// </summary>
    public ObservableCollection<Person> People { get; set; } = new ObservableCollection<Person>();

    public string BestAuthorDefaultIsNull
    {
        get
        {
            var personlist = from person in People orderby person.GetImportance() ascending select person;
            var author = personlist.FirstOrDefault();
            if (author == null)
            {
                return null;
            }
            return author.Name;
        }
    }

    /// <summary>
    /// Get a shortened title with author name suitable for being a filename.
    /// </summary>
    /// <returns></returns>
    public string GetBestTitleForFilename()
    {
        var personlist = from person in People orderby person.GetImportance() ascending select person;
        var author = personlist.FirstOrDefault();
        return TitleConverter(Title, author?.Name);
    }
    const int NICE_MIN_LEN = 20;
    const int NICE_MAX_LEN = 30;
    private string bookId;
    private string bookSource = BookSourceGutenberg;
    private FileType bookType = FileType.other;
    private string description;
    private string imprint;
    private string issued = "";
    private string title;
    private string titleAlternative;
    private string language;
    private string lCSH = "";
    private string lCCN = "";
    private string pGEditionInfo;
    private string pGProducedBy;
    private string pGNotes;
    private string bookSeries;
    private string lCC = "";
    private UserReview review = null;
    private BookNotes notes = null;
    private DownloadData downloadData = null;
    private BookNavigationData navigationData = null;
    private long denormDownloadDate; // unix time seconds

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

    /// <summary>
    /// Used by the TitleConverter to get a nice potential filename from the title+author.
    /// The file should include both title and author if possible
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private static string ChopString(string value, int min = NICE_MIN_LEN, int max = NICE_MAX_LEN)
    {
        if (value == null) return value;
        if (value.Length > min)
        {
            var nextspace = value.IndexOf(' ', min);
            if (nextspace < 0 && value.Length < max)
            {
                ; // no next space, but the total title isn't too long. Keep it as-is
            }
            else if (nextspace < 0)
            {
                // Too long, and no next space at all. Chop it ruthlessly.
                value = value.Substring(0, min);
            }
            else if (nextspace < max)
            {
                value = value.Substring(0, nextspace);
            }
            else
            {
                // Too long, and no convenient next space. Chop it ruthlessly.
                value = value.Substring(0, min);
            }
        }
        value = value.Trim();
        return value;
    }
    /// <summary>
    /// Given a title and author, generate a nice possible file string. Uses ASCII only (sorry, everyone
    /// with a name or title that doesn't convert)
    /// </summary>
    /// <param name="title"></param>
    /// <param name="author"></param>
    /// <returns></returns>
    private static string TitleConverter(string title, string author)
    {
        title = ChopString(title);
        author = ChopString(author);
        var potentialRetval = author == null ? title : $"{title}_by_{author}";
        potentialRetval = potentialRetval.Replace(" , ", ",").Replace(", ", ","); // because smith , jane is better as smith_jane
        char[] remove = { '\'', '.' };
        var sb = new StringBuilder();
        foreach (var ch in potentialRetval)
        {
            char c = ch;
            if (!remove.Contains(c))
            {
                if (char.IsControl(ch)) c = '-';
                else if (char.IsWhiteSpace(ch)) c = '_';
                else if (char.IsLetterOrDigit(ch)) c = ch;
                else if (ch < 128) c = '_';
                else c = ch; // allow all of the unicode chars
                sb.Append(c);
            }
        }
        var retval = sb.ToString();
        retval = retval.Trim('_');
        return retval;
    }

    /// <summary>
    /// List of all of the files for this book and their formats.
    /// </summary>
    public ObservableCollection<FilenameAndFormatData> Files { get; set; } = new ObservableCollection<FilenameAndFormatData>();
    /// <summary>
    /// <dcterms:language>
    ///     <rdf:Description rdf:nodeID="Nc3827dd334c44413ab159b8f40d432ec">
    ///         <rdf:value rdf:datatype="http://purl.org/dc/terms/RFC4646">en</rdf:value>
    ///     </rdf:Description>
    /// </dcterms:language>
    /// </summary>
    /// 
    public static bool FilesMatch(BookData a, BookData b)
    {
        var retval = true;
        foreach (var afile in a.Files)
        {
            if (FileIsKindle(afile.FileName)) continue; // Don't care about kindle files 
            var hasMatch = false;
            foreach (var bfile in b.Files)
            {
                if (afile.FileName == bfile.FileName)
                {
                    hasMatch = true;
                    break;
                }
            }
            if (!hasMatch)
            {
                retval = false;
                break;
            }
        }
        return retval;
    }
    public static bool FilesMatchEpub(BookData a, BookData b)
    {
        var retval = true;
        foreach (var afile in a.Files)
        {
            if (!FileIsEpub(afile.FileName)) continue; // Don't care about non-epub 
            var hasMatch = false;
            foreach (var bfile in b.Files)
            {
                if (afile.FileName == bfile.FileName)
                {
                    hasMatch = true;
                    break;
                }
            }
            if (!hasMatch)
            {
                retval = false;
                break;
            }
        }
        return retval;
    }
    public static bool FileIsEpub(string fname)
    {
        var retval = fname.EndsWith(".epub") || fname.Contains(".epub."); // why does Gutenberg do this :-(
        return retval;
    }
    public static bool FileIsKindle(string fname)
    {
        var retval = fname.Contains(".kindle.");
        return retval;
    }

    public static bool FilesIncludesEpub(BookData bd)
    {
        var retval = false;
        foreach (var afile in bd.Files)
        {
            if (FileIsEpub(afile.FileName))
            {
                retval = true;
                break; // Once I find one, that is good enough.
            }
        }
        return retval;
    }

    public string Language { get => language; set { if (language != value) { NotifyPropertyChanging(); language = value; NotifyPropertyChanged(); } } }
    // e.g. en. Apress raw data can be captialized as En, which IMHO is wrong.

    /// <summary>
    /// <dcterms:subject>
    ///     <rdf:Description rdf:nodeID="N0d26c4c9a07a454789d1f6545628914b">
    ///         <rdf:value>Cats -- Juvenile fiction</rdf:value>
    ///         <dcam:memberOf rdf:resource= "http://purl.org/dc/terms/LCSH" />
    ///     </rdf:Description>
    ///     </dcterms:subject>
    /// </summary>
    public string LCSH { get => lCSH; set { if (lCSH != value) { NotifyPropertyChanging(); lCSH = value; NotifyPropertyChanged(); } } }
    // is the Cats -- Juvenile fiction

    public string LCCN { get => lCCN; set { if (lCCN != value) { NotifyPropertyChanging(); lCCN = value; NotifyPropertyChanged(); } } }
    // Marc010 e.g. 18020634 is https://catalog.loc.gov/vwebv/search?searchArg=18020634&searchCode=GKEY%5E*&searchType=0&recCount=25&sk=en_US

    public string PGEditionInfo { get => pGEditionInfo; set { if (pGEditionInfo != value) { NotifyPropertyChanging(); pGEditionInfo = value; NotifyPropertyChanged(); } } }

    // Marc250
    public string PGProducedBy { get => pGProducedBy; set { if (pGProducedBy != value) { NotifyPropertyChanging(); pGProducedBy = value; NotifyPropertyChanged(); } } }

    // Marc508 e.g. Produced by Biblioteca Nacional Digital (http://bnd.bn.pt),\n
    public string PGNotes { get => pGNotes; set { if (pGNotes != value) { NotifyPropertyChanging(); pGNotes = value; NotifyPropertyChanged(); } } }

    // Marc546 e.g. This ebook uses a 19th century spelling for pg11299.rdf
    public string BookSeries { get => bookSeries; set { if (bookSeries != value) { NotifyPropertyChanging(); bookSeries = value; NotifyPropertyChanged(); } } }

    // Marc440 e.g. The Pony Rider Boys, number 8 for pg12980.rdf

    /// <summary>
    /// Example. Note that 'subject' might be LCC or LCSH
    /// <dcterms:subject>
    ///     <rdf:Description rdf:nodeID="N5e552155376c46acba0f56226354c4a8">
    ///         <dcam:memberOf rdf:resource="http://purl.org/dc/terms/LCC"/>
    ///         <rdf:value>PZ</rdf:value>
    ///     </rdf:Description>
    /// </dcterms:subject>
    /// 
    /// </summary>
    public string LCC { get => lCC; set { if (lCC != value) { NotifyPropertyChanging(); lCC = value; NotifyPropertyChanged(); } } }
    // is the PZ. Is a CSV because e.g. book 1 is both JK and E201

    //
    // Denormalized data used the make sorting go faster
    //
    public string DenormPrimaryAuthor { get; set; }
    public long DenormDownloadDate { get => denormDownloadDate; set { if (denormDownloadDate != value) { NotifyPropertyChanging(); denormDownloadDate = value; NotifyPropertyChanged(); } } }


    //
    // Next is all of the user-settable things
    //

    public UserReview Review { get => review; set { if (review != value) { NotifyPropertyChanging(); review = value; NotifyPropertyChanged(); } } }
    public BookNotes Notes { get => notes; set { if (notes != value) { NotifyPropertyChanging(); notes = value; NotifyPropertyChanged(); } } }
    public DownloadData DownloadData { get => downloadData; set { if (downloadData != value) { NotifyPropertyChanging(); downloadData = value; NotifyPropertyChanged(); } } }
    public BookNavigationData NavigationData { get => navigationData; set { if (navigationData != value) { NotifyPropertyChanging(); navigationData = value; NotifyPropertyChanged(); } } }
    // Used by the search system
    public IList<string> GetSearchArea(string inputArea)
    {
        var retval = new List<string>();
        var area = (inputArea + "...").Substring(0, 3).ToLower(); // e.g. title --> ti
        switch (area)
        {
            case "...":
                AddTitle(retval);
                AddPeople(retval);
                AddLCC(retval);
                AddNotes(retval);
                break;

            case "tit": // title
                AddTitle(retval);
                break;

            case "by.":
                AddPeople(retval);
                break;

            case "aut": // author is part of by
                foreach (var person in People)
                {
                    if (person.PersonType == Person.Relator.author
                        || person.PersonType == Person.Relator.artist
                        || person.PersonType == Person.Relator.collaborator
                        || person.PersonType == Person.Relator.contributor
                        || person.PersonType == Person.Relator.dubiousAuthor)
                    {
                        retval.Add(person.Name);
                        if (!string.IsNullOrEmpty(person.Aliases)) retval.Add(person.Aliases);
                    }
                }
                break;

            case "edi": // editor is part of by
                foreach (var person in People)
                {
                    if (person.PersonType == Person.Relator.editor
                        || person.PersonType == Person.Relator.editorOfCompilation
                        || person.PersonType == Person.Relator.printer
                        || person.PersonType == Person.Relator.publisher
                        )
                    {
                        retval.Add(person.Name);
                        if (!string.IsNullOrEmpty(person.Aliases)) retval.Add(person.Aliases);
                    }
                }
                break;

            case "lc.": // e.g. just the LCC=PS or the LCN=E305
                if (!string.IsNullOrEmpty(LCC)) retval.Add(LCC);
                if (!string.IsNullOrEmpty(LCCN)) retval.Add(LCCN);
                break;
            case "lcc": // LCC includes all LC
                AddLCC(retval);
                break;

            case "ill": // illustrator is part of by
                foreach (var person in People)
                {
                    if (person.PersonType == Person.Relator.illustrator
                        || person.PersonType == Person.Relator.artist
                        || person.PersonType == Person.Relator.engraver
                        || person.PersonType == Person.Relator.photographer)
                    {
                        retval.Add(person.Name);
                        if (!string.IsNullOrEmpty(person.Aliases)) retval.Add(person.Aliases);
                    }
                }
                break;

            case "not": // notes and reviews
                AddNotes(retval);
                break;

            case "ser": // series, like the Pony Rider (is part of title)
                if (!string.IsNullOrEmpty(BookSeries)) retval.Add(BookSeries);
                break;

            case "sub": // lcc subject headings e.g. just the LCC=PS or the LCN=E305
                if (!string.IsNullOrEmpty(LCSH)) retval.Add(LCSH);
                break;
        }
        return retval;
    }

    private void AddLCC(List<string> retval)
    {
        if (!string.IsNullOrEmpty(LCC)) retval.Add(LCC);
        if (!string.IsNullOrEmpty(LCCN)) retval.Add(LCCN);
        if (!string.IsNullOrEmpty(LCSH)) retval.Add(LCSH);
    }

    private void AddNotes(List<string> retval)
    {
        if (!string.IsNullOrEmpty(Review?.Tags)) retval.Add(Review.Tags);
        if (!string.IsNullOrEmpty(Review?.Review)) retval.Add(Review.Review);
        if (Notes != null && Notes.Notes != null && Notes.Notes.Count > 0)
        {
            foreach (var note in Notes.Notes)
            {
                if (!string.IsNullOrEmpty(note.Tags)) retval.Add(note.Tags);
                if (!string.IsNullOrEmpty(note.Text)) retval.Add(note.Text);
            }
        }
    }
    private void AddPeople(List<string> retval)
    {
        foreach (var person in People)
        {
            retval.Add(person.Name);
            if (!string.IsNullOrEmpty(person.Aliases)) retval.Add(person.Aliases);
        }
    }
    private void AddTitle(List<string> retval)
    {
        retval.Add(Title);
        if (!string.IsNullOrEmpty(TitleAlternative)) retval.Add(TitleAlternative);
        if (!string.IsNullOrEmpty(BookSeries)) retval.Add(BookSeries);
    }
    /// <summary>
    /// Merge two book data items together where one is directly from a catalog and has
    /// no user data (like a review or notes)
    /// TODO: finish!
    /// </summary>
    /// <param name="existing"></param>
    /// <param name="catalog"></param>
    public static void Merge(BookData existing, BookData catalog)
    {
        // book id: keep existing
        existing.BookSource = catalog.BookSource;
        existing.BookType = catalog.BookType;
        existing.Description = catalog.Description;
        existing.Imprint = catalog.Imprint;
        existing.Issued = catalog.Issued;
        existing.Title = catalog.Title;
        existing.TitleAlternative = catalog.TitleAlternative;
        while (existing.People.Count >0)
        {
            existing.People.RemoveAt(0);
        }
        // To heck with collections that can't clear! existing.People.Clear();
        foreach (var person in catalog.People)
        {
            if (person.Id != 0) person.Id = 0; // Straight from a catalog there should be no person id values set.
            existing.People.Add(person);
        }
        while (existing.Files.Count > 0)
        {
            existing.Files.RemoveAt(0);
        }
        // to heck with...existing.Files.Clear();
        foreach (var file in catalog.Files)
        {
            if (file.Id != 0) file.Id = 0; // Straight from a catalog there should be no file id values set.
            existing.Files.Add(file);
        }
        existing.Language = catalog.Language;
        existing.LCSH = catalog.LCSH;
        existing.LCCN = catalog.LCCN;
        existing.PGEditionInfo = catalog.PGEditionInfo;
        existing.PGProducedBy = catalog.PGProducedBy;
        existing.PGNotes = catalog.PGNotes;
        existing.BookSeries = catalog.BookSeries;
        existing.LCC = catalog.LCC;
        existing.DenormPrimaryAuthor = catalog.DenormPrimaryAuthor;
    }
    public override string ToString()
    {
        return $"{Title.Substring(0, Math.Min(Title.Length, 20))} for {BookId}";
    }
}
