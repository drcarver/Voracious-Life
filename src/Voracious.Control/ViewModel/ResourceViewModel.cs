using System.Text;

using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.Core.Enum;
using Voracious.Core.Interface;
using Voracious.Core.Model;

namespace Voracious.Control.ViewModel;

/// <summary>
/// One Gutenberg record for a book (not all data is saved)
/// </summary>
public partial class ResourceViewModel : ObservableObject, IResourceCore, IGetSearchAreaCore
{
    private const string BookSourceGutenberg = "Project Gutenberg";
    private const int NICE_MIN_LEN = 20;
    private const int NICE_MAX_LEN = 30;

    #region observable properties
    /// <summary>
    /// The primary key
    /// </summary>
    [ObservableProperty]
    private string about;

    /// <summary>
    /// An alternative name for the resource.
    /// </summary>
    [ObservableProperty]
    private string? titleAlternative;

    /// <summary>
    /// The book Series
    /// </summary>
    [ObservableProperty]
    private string bookSeries;

    /// <summary>
    /// The nature or genre of the resource.
    /// <para>
    /// dc:type
    /// </para>
    /// </summary>
    /// <remarks>
    /// Recommended best practice is to use a controlled vocabulary
    /// such as the DCMI Type Vocabulary[DCMITYPE]. To describe the 
    /// file format, physical medium, or dimensions of the resource, 
    /// </remarks>
    [ObservableProperty]
    private FileTypeEnum bookType;

    /// <summary>
    /// An entity responsible for making contributions to the resource.
    /// <para>
    /// dc:contributor
    /// </para>
    /// <para>
    /// An unordered array of ProperName
    /// </para>
    /// </summary>
    /// <remarks>
    /// Examples of a contributor include a creator, an organization, or
    /// a service.Typically, the name of a contributor should be used to 
    /// indicate the entity. XMP addition: XMP usage is a list of contributors. 
    /// These contributors should not include those listed in dc:creator.
    /// </remarks>
    [ObservableProperty]
    private List<CreatorViewModel> contributors;

    /// <summary>
    /// The spatial or temporal topic of the resource, the spatial 
    /// applicability of the resource, or the jurisdiction under 
    /// which the resource is relevant.
    /// <para>
    /// dc:coverage 
    /// </para>
    /// <para>
    /// Text
    /// </para>
    /// </summary>
    /// <remarks>
    /// XMP usage is the extent or scope of the resource.
    /// </remarks>
    [ObservableProperty]
    private List<string> coverages;

    /// <summary>
    /// Credits for creators or organizations, other than members of the cast, who 
    /// have participated in the creation and/or production of the work. 
    /// The introductory term Credits: is usually generated as a display constant.
    /// </summary>
    /// <remarks>
    /// Marc508 e.g. Produced by Biblioteca Nacional Digital (http://bnd.bn.pt),
    /// </remarks>
    [ObservableProperty]
    private string? creationProductionCreditsNote;

    /// <summary>
    /// An account of the resource.
    /// <para>
    /// dc:description
    /// </para>
    /// <para>
    /// Language Alternative
    /// </para>
    /// </summary>
    [ObservableProperty]
    private string? description;

    /// <summary>
    /// The number of times the book was downloaded
    /// </summary>
    [ObservableProperty]
    private int? downloads;

    /// <summary>
    /// The file format, physical medium, or dimensions of the resource.
    /// <para>
    /// dc:format 
    /// </para>
    /// <para>
    /// MIMEType 
    /// </para>
    /// </summary>
    /// <remarks>
    /// Examples of dimensions include size and duration. Recommended best 
    /// practice is to use a controlled vocabulary such as the list of
    /// Internet Media Types[MIME]
    /// </remarks>
    [ObservableProperty]
    private List<FileFormatViewModel> formats = [];

    /// <summary>
    ///  A name given to the resource.
    /// <para>
    /// dc:title
    /// </para>
    /// <para>
    /// Language Alternative
    /// </para>
    /// </summary>
    /// <remarks>
    /// Typically, a title will be a name by which the resource is formally known.
    /// </remarks>
    [ObservableProperty]
    private string title;

    /// <summary>
    /// The topic of the resource.
    /// <para>
    /// dc:subject
    /// </para>
    /// <para>
    /// Unordered array of Text
    /// </para>
    /// </summary>
    /// <remarks>
    /// Typically, the subject will be represented using keywords, key
    /// phrases, or classification codes.Recommended best practice is 
    /// to use a controlled vocabulary. To describe the spatial or 
    /// temporal topic of the resource, use the dc:coverage element.
    /// </remarks>
    [ObservableProperty]
    private List<string> subjects;

    /// <summary>
    /// An entity responsible for making the resource available.
    /// <para>
    /// dc:publisher
    /// </para>
    /// </summary>
    /// <remarks>
    /// Examples of a publisher include a creator, an organization, or a
    /// service. Typically, the name of a publisher should be used to indicate 
    /// the entity.
    /// </remarks>
    [ObservableProperty]
    private string? publisher;

    /// </summary>
    /// <summary>
    /// Date of formal issuance of the resource.
    /// </summary>
    /// <remarks>
    /// <dcterms:issued rdf:datatype="http://www.w3.org/2001/XMLSchema#date">1994-07-01</dcterms:issued>
    /// </remarks>
    [ObservableProperty]
    private DateTime issued;

    /// <summary>
    /// A related resource from which the described resource is derived.
    /// <para>
    /// dc:source 
    /// </para>
    /// <para>
    /// Text 
    /// </para>
    /// </summary>
    /// <remarks>
    /// The described resource may be derived from the related resource in 
    /// whole or in part.Recommended best practice is to identify the related
    /// resource by means of a string conforming to a formal identification system.
    /// </remarks>
    [ObservableProperty]
    private string sources;

    /// <summary>
    /// A language of the resource.
    /// <para>
    /// dc:language
    /// </para>
    /// </summary>
    [ObservableProperty]
    private string language;

    /// <summary>
    /// The set of conceptual resources specified by the Library of Congress
    /// Classification.
    /// </summary>
    [ObservableProperty]
    private string lCC;

    /// <summary>
    /// A Library of Congress catalog control number is a unique identification 
    /// number that the Library of Congress assigns to the catalog record 
    /// created for each book in its cataloged collections.
    /// </summary>
    [ObservableProperty]
    private string lCCN;

    /// <summary>
    /// Library of Congress Subject Headings (LCSH) has been actively maintained 
    /// since 1898 to catalog materials held at the Library of Congress. 
    /// </summary>
    [ObservableProperty]
    private string lCSH;

    /// <summary>
    /// A legal document giving official permission to do something with 
    /// the resource.
    /// </summary>
    /// <remarks>
    /// <dcterms:license rdf:resource="license"/>
    /// </remarks>
    [ObservableProperty]
    private string? license;

    /// <summary>
    /// A related resource.
    /// <para>
    /// dc:relation
    /// </para>
    /// <para>
    /// Unordered array of Text
    /// </para>
    /// </summary>
    /// <remarks>
    /// Recommended best practice is to identify the related resource
    /// by means of a string conforming to a formal identification system.
    /// </remarks>
    [ObservableProperty]
    private List<string> relations;

    /// <summary>
    /// Information about rights held in and over the resource.
    /// <para>
    /// dc:rights
    /// </para>
    /// <para>
    /// Language Alternative
    /// </para>
    /// </summary>
    /// <remarks>
    /// Typically, rights information includes a statement about various
    /// property rights associated with the resource, including intellectual 
    /// property rights.
    /// </remarks>
    [ObservableProperty]
    private string? rights;

    /// <summary>
    /// marc260: Houston: Advantage International, The PaperLess Readers Club, 1992
    /// </summary>
    [ObservableProperty]
    private string? imprint;

    /// <summary>
    /// Project Gutenberg Edition information
    /// </summary>
    [ObservableProperty]
    private string pGEditionInfo;

    /// <summary>
    /// Project Gutenberg Produced By
    /// </summary>
    [ObservableProperty]
    private string pGProducedBy;
    #endregion

    #region navigation properties and commands
    /// <summary>
    /// An entity primarily responsible for making the resource
    /// </summary>
    /// <remarks>
    /// A second property with the same name as this property has been declared in 
    /// the dcterms: namespace. See the Introduction to the document DCMI Metadata 
    /// Terms for an explanation.
    /// </remarks>
    [ObservableProperty]
    private List<CreatorViewModel> creators = [];

    /// <summary>
    /// List of all of the files for this book and their formats.
    /// </summary>
    [ObservableProperty]
    public List<FileFormatViewModel> files = [];

    ////
    //// Next is all of the user-settable things
    ////
    ////[ObservableProperty]
    ////private UserReviewViewModel? review;

    ////[ObservableProperty]
    ////private List<UserNoteViewModel>? notes;

    ////[ObservableProperty]
    ////private BookNavigationViewModel? navigationData;
    #endregion

    /// <summary>
    /// Return either "" (is valid) or a loggable string of why
    /// the book has problems.
    /// </summary>
    /// <returns></returns>
    public string Validate()
    {
        var retval = "";
        if (string.IsNullOrWhiteSpace(About))
        {
            retval += "ERROR: BookId is not set\n";
        }
        if (string.IsNullOrWhiteSpace(Title)) retval += "ERROR: Title is not set\n";
        if (!string.IsNullOrWhiteSpace(TitleAlternative) && string.IsNullOrWhiteSpace(Title)) retval += "ERROR: TitleAlternative is set but Title is not\n";

        if (Issued == null)
        {
            retval += "ERROR: Book was not issued\n";
        }

        //if (Title == "No title" && Files.Count == 0)
        //{
        //    retval += "ERROR: Gutenberg made a book with no title or files";
        //}

        //if (retval != "" && Files.Count == 0)
        //{
        //    retval += "ERROR: Book has no files"; 
        //}
        return retval;
    }

    //public bool FilesMatch(ResourceViewModel a, ResourceViewModel b)
    //{
    //    var retval = true;
    //    foreach (var afile in a.Files)
    //    {
    //        if (FileIsKindle(afile.FileName)) continue; // Don't care about kindle files 
    //        var hasMatch = false;
    //        foreach (var bfile in b.Files)
    //        {
    //            if (afile.FileName == bfile.FileName)
    //            {
    //                hasMatch = true;
    //                break;
    //            }
    //        }
    //        if (!hasMatch)
    //        {
    //            retval = false;
    //            break;
    //        }
    //    }
    //    return retval;
    //}

    //public bool FilesMatchEpub(ResourceViewModel a, ResourceViewModel b)
    //{
    //    var retval = true;
    //    foreach (var afile in a.Files)
    //    {
    //        if (!FileIsEpub(afile.FileName)) continue; // Don't care about non-epub 
    //        var hasMatch = false;
    //        foreach (var bfile in b.Files)
    //        {
    //            if (afile.FileName == bfile.FileName)
    //            {
    //                hasMatch = true;
    //                break;
    //            }
    //        }
    //        if (!hasMatch)
    //        {
    //            retval = false;
    //            break;
    //        }
    //    }
    //    return retval;
    //}

    public bool FileIsEpub(string fname)
    {
        return fname.EndsWith(".epub") || fname.Contains(".epub."); // why does Gutenberg do this :-(
    }

    public bool FileIsKindle(string fname)
    {
        return fname.Contains(".kindle.");
    }

    //public bool FilesIncludesEpub(ResourceViewModel bd)
    //{
    //    var retval = false;
    //    foreach (var afile in bd.Files)
    //    {
    //        if (FileIsEpub(afile.FileName))
    //        {
    //            retval = true;
    //            break; // Once I find one, that is good enough.
    //        }
    //    }
    //    return retval;
    //}

    // Used by the search system

    public List<string> GetSearchArea(string inputArea)
    {
        var retval = new List<string>();
        var area = (inputArea + "...").Substring(0, 3).ToLower(); // e.g. title --> ti
        switch (area)
        {
            case "...":
                AddTitle(retval);
                AddPeople(retval);
                AddLCC(retval);
                //AddNotes(retval);
                break;

            case "tit": // title
                AddTitle(retval);
                break;

            case "by.":
                AddPeople(retval);
                break;

            case "aut": // author is part of by
                foreach (var creator in Creators)
                {
                    if (creator.Role == RelatorEnum.author
                        || creator.Role == RelatorEnum.artist
                        || creator.Role == RelatorEnum.collaborator
                        || creator.Role == RelatorEnum.contributor
                        || creator.Role == RelatorEnum.dubiousAuthor)
                    {
                        retval.Add(creator.Name);
                        if (!string.IsNullOrEmpty(creator.Aliases)) retval.Add(creator.Aliases);
                    }
                }
                break;

            case "edi": // editor is part of by
                foreach (var creator in Creators)
                {
                    if (creator.Role == RelatorEnum.editor
                        || creator.Role == RelatorEnum.editorOfCompilation
                        || creator.Role == RelatorEnum.printer
                        || creator.Role == RelatorEnum.publisher
                        )
                    {
                        retval.Add(creator.Name);
                        if (!string.IsNullOrEmpty(creator.Aliases)) retval.Add(creator.Aliases);
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
                foreach (var creator in Creators)
                {
                    if (creator.Role == RelatorEnum.illustrator
                        || creator.Role == RelatorEnum.artist
                        || creator.Role == RelatorEnum.engraver
                        || creator.Role == RelatorEnum.photographer)
                    {
                        retval.Add(creator.Name);
                        if (!string.IsNullOrEmpty(creator.Aliases)) retval.Add(creator.Aliases);
                    }
                }
                break;

            case "not": // notes and reviews
                //AddNotes(retval);
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

    //private void AddNotes(List<string> retval)
    //{
    //    if (!string.IsNullOrEmpty(Review?.Tags)) retval.Add(Review.Tags);
    //    if (Notes != null && Notes.Count > 0)
    //    {
    //        foreach (var note in Notes)
    //        {
    //            if (!string.IsNullOrEmpty(note.Tags)) retval.Add(note.Tags);
    //            if (!string.IsNullOrEmpty(note.Text)) retval.Add(note.Text);
    //        }
    //    }
    //}

    private void AddPeople(List<string> retval)
    {
        foreach (var creator in Creators)
        {
            retval.Add(creator.Name);
            if (!string.IsNullOrEmpty(creator.Aliases)) retval.Add(creator.Aliases);
        }
    }

    private void AddTitle(List<string> retval)
    {
        retval.Add(Title);
        if (!string.IsNullOrEmpty(TitleAlternative)) retval.Add(TitleAlternative);
        if (!string.IsNullOrEmpty(BookSeries)) retval.Add(BookSeries);
    }

    public string? BestAuthorDefaultIsNull => Creators.OrderBy(p => p.GetImportance()).FirstOrDefault()?.Name;

    public string FileAs { get; set; }
    FileTypeEnum? IResourceCore.BookType { get; set; }

    /// <summary>
    /// Get a shortened title with author name suitable for being a filename.
    /// </summary>
    /// <returns></returns>
    public string GetBestTitleForFilename()
    {
        return TitleConverter(Title, BestAuthorDefaultIsNull);
    }

    /// <summary>
    /// Used by the TitleConverter to get a nice potential filename from the title+author.
    /// The file should include both title and author if possible
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private string? ChopString(string value, int min = NICE_MIN_LEN, int max = NICE_MAX_LEN)
    {
        if (value == null)
        {
            return value;
        }
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
    /// Given a title and author, generate a nice possible file string. Uses ASCII 
    /// only (sorry, everyone with a name or title that doesn't convert)
    /// </summary>
    /// <param name="title"></param>
    /// <param name="author"></param>
    /// <returns></returns>
    private string TitleConverter(string? title, string? author)
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
}
