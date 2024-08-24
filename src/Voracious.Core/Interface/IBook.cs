using System;
using System.Collections.ObjectModel;

using Voracious.Core.Enum;
using Voracious.Core.ViewModel;

namespace Voracious.Core.Interface;

/// <summary>
/// One Gutenberg record for a book (not all data is saved)
/// </summary>
public interface IBook
{
    string? BookId { get; set; }

    string? BookSource { get; set; }

    FileTypeEnum BookType { get; set; }

    /// <summary>
    /// Examples:
    /// <dcterms:description>There is an improved edition of this title, eBook #29888</dcterms:description>
    /// <dcterms:description>Illustrated by the author.</dcterms:description>
    /// </summary>
    string? Description { get; set; }

    /// <summary>
    /// Examples:
    /// #28: <pgterms:marc260>Houston: Advantage International, The PaperLess Readers Club, 1992</pgterms:marc260>
    /// </summary>
    string? Imprint { get; set; }

    DateTime? Issued { get; set; }

    /// <summary>
    /// <dcterms:title>Three Little Kittens</dcterms:title>
    /// </summary>
    string? Title { get; set; }

    string? TitleAlternative { get; set; }

    /// <summary>
    /// People include authors, illustrators, etc.
    /// </summary>
    ObservableCollection<PersonViewModel> People { get; set; }

    /// <summary>
    /// List of all of the files for this book and their formats.
    /// </summary>
    ObservableCollection<FilenameAndFormatDataViewModel> Files { get; set; }

    /// <summary>
    /// e.g. en. A press raw data can be capitalized as En, which IMHO is wrong.
    /// </summary>
    string Language { get; set; }

    /// <summary>
    /// <dcterms:subject>
    ///     <rdf:Description rdf:nodeID="N0d26c4c9a07a454789d1f6545628914b">
    ///         <rdf:value>Cats -- Juvenile fiction</rdf:value>
    ///         <dcam:memberOf rdf:resource= "http://purl.org/dc/terms/LCSH" />
    ///     </rdf:Description>
    ///     </dcterms:subject>
    /// </summary>
    string LCSH { get; set; }

    /// <summary>
    /// Marc010 e.g. 18020634 is 
    /// https://catalog.loc.gov/vwebv/search?searchArg=18020634&searchCode=GKEY%5E*&searchType=0&recCount=25&sk=en_US
    /// </summary>
    string LCCN { get; set; }

    string PGEditionInfo { get; set; }

    // Marc25L
    string PGProducedBy { get; set; }

    /// <summary>
    /// Marc508 e.g. Produced by Biblioteca Nacional Digital (http://bnd.bn.pt),
    /// </summary>
    string PGNotes { get; set; }

    /// <summary>
    /// Marc546 e.g. This ebook uses a 19th century spelling for pg11299.rdf
    /// Marc440 e.g. The Pony Rider Boys, number 8 for pg12980.rdf
    /// </summary>
    string BookSeries { get; set; }

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
    string LCC { get; set; }

    long DenormDownloadDate { get; set; }

    //
    // De normalized data used the make sorting go faster
    //
    string DenormPrimaryAuthor { get; set; }

    //
    // Next is all of the user-settable things
    //
    UserReviewViewModel Review { get; set; }

    BookNoteViewModel Notes { get; set; }

    DownloadDataViewModel DownloadData { get; set; }

    BookNavigationViewModel NavigationData { get; set; }
}
