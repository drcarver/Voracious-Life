using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.Core.Enum;
using Voracious.Core.Interface;
using Voracious.Core.Model;

namespace Voracious.Core.ViewModel;

/// <summary>
/// One Gutenberg record for a book (not all data is saved)
/// </summary>
public partial class ResourceViewModel : ObservableObject, IResource, IGetSearchArea
{
    private const string BookSourceGutenberg = "Project Gutenberg";
    private const int NICE_MIN_LEN = 20;
    private const int NICE_MAX_LEN = 30;

    #region observable properties
    // remarks are from <pgterms:ebook rdf:about="ebooks/147">

    /// <summary>
    /// Sets the subject URI of a statement, which may be absolute
    /// </summary>
    /// <remarks>
    /// <pgterms:ebook rdf:about="ebooks/147">
    /// </remarks>
    [ObservableProperty]
    private string about = string.Empty;

    /// <summary>
    /// An entity responsible for making the resource available.
    /// </summary>
    /// <remarks>
    /// <dcterms:publisher>Project Gutenberg</dcterms:publisher>
    /// </remarks>
    [ObservableProperty]
    private string? publisher = BookSourceGutenberg;

    /// <summary>
    /// An account of the resource. Description may include but is not limited to: 
    /// an abstract, a table of contents, a graphical representation, or a free-text 
    /// account of the resource
    /// </summary>
    /// <remarks>
    /// <dcterms:description></dcterms:description>
    /// </remarks>
    [ObservableProperty]
    private string? description = string.Empty;

    /// <summary>
    /// A legal document giving official permission to do something with the resource.
    /// </summary>
    /// <remarks>
    /// <dcterms:license rdf:resource="license"/>
    /// </remarks>
    [ObservableProperty]
    private string? license = "license";

    /// <summary>
    /// Date of formal issuance of the resource.
    /// </summary>
    /// <remarks>
    /// <dcterms:issued rdf:datatype="http://www.w3.org/2001/XMLSchema#date">1994-07-01</dcterms:issued>
    /// </remarks>
    [ObservableProperty]
    private DateTime issued = DateTime.UtcNow.Date;

    /// <summary>
    /// Information about rights held in and over the resource. Typically, rights 
    /// information includes a statement about various property rights associated 
    /// with the resource, including intellectual property rights.
    /// </summary>
    /// <remarks>
    /// <dcterms:rights>Public domain in the USA.</dcterms:rights>
    /// </remarks>
    [ObservableProperty]
    private string? rights;

    /// <summary>
    /// The number of downloads for this resource. 
    /// </summary>
    /// <remarks>
    /// <pgterms:downloads rdf:datatype="http://www.w3.org/2001/XMLSchema#integer">4641</pgterms:downloads>
    /// </remarks>
    [ObservableProperty]
    private int? downloads;

    /// <summary>
    /// A name given to the resource.
    /// </summary>
    /// <remarks>
    /// <dcterms:title>Common Sense</dcterms:title>
    /// </remarks>
    [ObservableProperty]
    private string title = string.Empty;

    /// <summary>
    /// Credits for persons or organizations, other than members of the cast, who 
    /// have participated in the creation and/or production of the work. 
    /// The introductory term Credits: is usually generated as a display constant.
    /// </summary>
    /// <remarks>
    /// Marc508 e.g. Produced by Biblioteca Nacional Digital (http://bnd.bn.pt),
    /// </remarks>
    [ObservableProperty]
    private string? creationProductionCreditsNote;

    /// <summary>
    /// A cultureInfo for the language.
    /// </summary>
    //[ObservableProperty]
    //[property: IgnoreDataMember]
    //private CultureInfo languageCultureInfo = new CultureInfo("en");

    /// <summary>
    /// A language of the resource.
    /// </summary>
    /// <remarks>
    ///     <dcterms:language>
    ///         <rdf:Description rdf:nodeID="Nc3827dd334c44413ab159b8f40d432ec">
    ///             <rdf:value rdf:datatype="http://purl.org/dc/terms/RFC4646">en</rdf:value>
    ///         </rdf:Description>
    ///     </dcterms:language>
    /// </remarks>
    [ObservableProperty]
    private string language = "en";

    [ObservableProperty]
    private FileTypeEnum bookType;

    /// <summary>
    /// Examples:
    /// #28: <pgterms:marc260>Houston: Advantage International, The PaperLess Readers Club, 1992</pgterms:marc260>
    /// </summary>
    [ObservableProperty]
    private string? imprint;

    [ObservableProperty]
    private string? titleAlternative;

    /// <summary>
    /// <dcterms:subject>
    ///     <rdf:Description rdf:nodeID="N0d26c4c9a07a454789d1f6545628914b">
    ///         <rdf:value>Cats -- Juvenile fiction</rdf:value>
    ///         <dcam:memberOf rdf:resource= "http://purl.org/dc/terms/LCSH" />
    ///     </rdf:Description>
    ///     </dcterms:subject>
    /// </summary>
    [ObservableProperty]
    private string? lCSH;

    /// <summary>
    /// Marc010 e.g. 18020634 is 
    /// https://catalog.loc.gov/vwebv/search?searchArg=18020634&searchCode=GKEY%5E*&searchType=0&recCount=25&sk=en_US
    /// </summary>
    [ObservableProperty]
    private string? lCCN;

    [ObservableProperty]
    private string? pGEditionInfo;

    // Marc250
    [ObservableProperty]
    private string? pGProducedBy;

    /// <summary>
    /// Marc546 e.g. This ebook uses a 19th century spelling for pg11299.rdf
    /// Marc440 e.g. The Pony Rider Boys, number 8 for pg12980.rdf
    /// </summary>
    [ObservableProperty]
    private string? bookSeries;

    /// <summary>
    /// Example. Note that 'subject' might be LCC or LCSH
    /// <dcterms:subject>
    ///     <rdf:Description rdf:nodeID="N5e552155376c46acba0f56226354c4a8">
    ///         <dcam:memberOf rdf:resource="http://purl.org/dc/terms/LCC"/>
    ///         <rdf:value>PZ</rdf:value>
    ///     </rdf:Description>
    /// </dcterms:subject>
    /// is the PZ. Is a CSV because e.g. book 1 is both JK and E201
    /// </summary>
    [ObservableProperty]
    private string? lCC;
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
    private List<IPerson> people = [];

    /// <summary>
    /// List of all of the files for this book and their formats.
    /// </summary>
    [ObservableProperty]
    public List<FilenameAndFormatDataViewModel> files = [];

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
                //AddNotes(retval);
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
                    if (person.Relator == RelatorEnum.author
                        || person.Relator == RelatorEnum.artist
                        || person.Relator == RelatorEnum.collaborator
                        || person.Relator == RelatorEnum.contributor
                        || person.Relator == RelatorEnum.dubiousAuthor)
                    {
                        retval.Add(person.Name);
                        if (!string.IsNullOrEmpty(person.Aliases)) retval.Add(person.Aliases);
                    }
                }
                break;

            case "edi": // editor is part of by
                foreach (var person in People)
                {
                    if (person.Relator == RelatorEnum.editor
                        || person.Relator == RelatorEnum.editorOfCompilation
                        || person.Relator == RelatorEnum.printer
                        || person.Relator == RelatorEnum.publisher
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
                    if (person.Relator == RelatorEnum.illustrator
                        || person.Relator == RelatorEnum.artist
                        || person.Relator == RelatorEnum.engraver
                        || person.Relator == RelatorEnum.photographer)
                    {
                        retval.Add(person.Name);
                        if (!string.IsNullOrEmpty(person.Aliases)) retval.Add(person.Aliases);
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

    public string? BestAuthorDefaultIsNull => People.Cast<PersonModel>().OrderBy(p => p.GetImportance()).FirstOrDefault()?.Name;

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
