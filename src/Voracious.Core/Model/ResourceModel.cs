using System;
using System.Collections.Generic;

using Voracious.Core.Enum;
using Voracious.Core.Interface;

namespace Voracious.Core.Model;

/// <summary>
/// One Gutenberg record for a book (not all data is saved)
/// </summary>
public partial class ResourceModel : IResource
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
    public string About { get; set; } = string.Empty;

    /// <summary>
    /// An entity responsible for making the resource available.
    /// </summary>
    /// <remarks>
    /// <dcterms:publisher>Project Gutenberg</dcterms:publisher>
    /// </remarks>
    public string? Publisher { get; set; } = BookSourceGutenberg;

    /// <summary>
    /// An account of the resource. Description may include but is not limited to: 
    /// an abstract, a table of contents, a graphical representation, or a free-text 
    /// account of the resource
    /// </summary>
    /// <remarks>
    /// <dcterms:description></dcterms:description>
    /// </remarks>
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// A legal document giving official permission to do something with the resource.
    /// </summary>
    /// <remarks>
    /// <dcterms:license rdf:resource="license"/>
    /// </remarks>
    public string? License { get; set; } = "license";

    /// <summary>
    /// Date of formal issuance of the resource.
    /// </summary>
    /// <remarks>
    /// <dcterms:issued rdf:datatype="http://www.w3.org/2001/XMLSchema#date">1994-07-01</dcterms:issued>
    /// </remarks>
    public DateTime Issued { get; set; } = DateTime.Now.Date;

    /// <summary>
    /// Information about rights held in and over the resource. Typically, rights 
    /// information includes a statement about various property rights associated 
    /// with the resource, including intellectual property rights.
    /// </summary>
    /// <remarks>
    /// <dcterms:rights>Public domain in the USA.</dcterms:rights>
    /// </remarks>
    public string? Rights {  get; set; }

    /// <summary>
    /// The number of downloads for this resource. 
    /// </summary>
    /// <remarks>
    /// <pgterms:downloads rdf:datatype="http://www.w3.org/2001/XMLSchema#integer">4641</pgterms:downloads>
    /// </remarks>
    public int? Downloads { get; set; }

    /// <summary>
    /// A name given to the resource.
    /// </summary>
    /// <remarks>
    /// <dcterms:title>Common Sense</dcterms:title>
    /// </remarks>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Credits for persons or organizations, other than members of the cast, who 
    /// have participated in the creation and/or production of the work. 
    /// The introductory term Credits: is usually generated as a display constant.
    /// </summary>
    /// <remarks>
    /// Marc508 e.g. Produced by Biblioteca Nacional Digital (http://bnd.bn.pt),
    /// </remarks>
    public string? CreationProductionCreditsNote { get; set; }

    /// <summary>
    /// A cultureInfo for the language.
    /// </summary>
    //[ObservableProperty]
    //[property: IgnoreDataMember]
    //public CultureInfo languageCultureInfo = new CultureInfo("en");

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
    public string Language { get; set; } = "en";

    public FileTypeEnum BookType { get; set; }

    /// <summary>
    /// Examples:
    /// #28: <pgterms:marc260>Houston: Advantage International, The PaperLess Readers Club, 1992</pgterms:marc260>
    /// </summary>
    public string? Imprint { get; set; }

    public string? TitleAlternative {  get; set; }

    /// <summary>
    /// <dcterms:subject>
    ///     <rdf:Description rdf:nodeID="N0d26c4c9a07a454789d1f6545628914b">
    ///         <rdf:value>Cats -- Juvenile fiction</rdf:value>
    ///         <dcam:memberOf rdf:resource= "http://purl.org/dc/terms/LCSH" />
    ///     </rdf:Description>
    ///     </dcterms:subject>
    /// </summary>
    public string? LCSH { get; set; }

    /// <summary>
    /// Marc010 e.g. 18020634 is 
    /// https://catalog.loc.gov/vwebv/search?searchArg=18020634&searchCode=GKEY%5E*&searchType=0&recCount=25&sk=en_US
    /// </summary>
    public string? LCCN { get; set; }

    public string? PGEditionInfo { get; set; }

    // Marc250
    public string? PGProducedBy { get; set; }

    /// <summary>
    /// Marc546 e.g. This ebook uses a 19th century spelling for pg11299.rdf
    /// Marc440 e.g. The Pony Rider Boys, number 8 for pg12980.rdf
    /// </summary>
    public string? BookSeries {  get; set; }

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
    public string? LCC {  get; set; }
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
    public List<PersonModel> People { get; set; } = [];

    /// <summary>
    /// List of all of the files for this book and their formats.
    /// </summary>
    //public List<FilenameAndFormatDataModel> Files { get; set; } = [];

    ////
    //// Next is all of the user-settable things
    ////
    ////[ObservableProperty]
    ////public UserReviewViewModel? review;

    ////[ObservableProperty]
    ////public List<UserNoteViewModel>? notes;

    ////[ObservableProperty]
    ////public BookNavigationViewModel? navigationData;
    #endregion
}
