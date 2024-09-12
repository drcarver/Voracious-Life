using System;
using System.Collections.Generic;

using Voracious.Core.Enum;
using Voracious.Core.Model;

namespace Voracious.Core.Interface;

/// <summary>
/// One Gutenberg record for a book (not all data is saved)
/// </summary>
public interface IResource
{
    /// <summary>
    /// Sets the subject URI of a statement, which may be absolute
    /// </summary>
    /// <remarks>
    /// <pgterms:ebook rdf:about="ebooks/147">
    /// </remarks>
    string About { get; set; }

    /// <summary>
    /// An entity responsible for making the resource available.
    /// </summary>
    /// <remarks>
    /// <dcterms:publisher>Project Gutenberg</dcterms:publisher>
    /// </remarks>
    string? Publisher { get; set; }

    /// <summary>
    /// An account of the resource. Description may include but is not limited to: 
    /// an abstract, a table of contents, a graphical representation, or a free-text 
    /// account of the resource
    /// </summary>
    /// <remarks>
    /// <dcterms:description></dcterms:description>
    /// </remarks>
    string? Description { get; set; }

    /// <summary>
    /// A legal document giving official permission to do something with the resource.
    /// </summary>
    /// <remarks>
    /// <dcterms:license rdf:resource="license"/>
    /// </remarks>
    string? License { get; set; }

    /// <summary>
    /// Date of formal issuance of the resource.
    /// </summary>
    /// <remarks>
    /// <dcterms:issued rdf:datatype="http://www.w3.org/2001/XMLSchema#date">1994-07-01</dcterms:issued>
    /// </remarks>
    DateTime Issued { get; set; }

    /// <summary>
    /// Information about rights held in and over the resource. Typically, rights 
    /// information includes a statement about various property rights associated 
    /// with the resource, including intellectual property rights.
    /// </summary>
    /// <remarks>
    /// <dcterms:rights>Public domain in the USA.</dcterms:rights>
    /// </remarks>
    string? Rights { get; set; }

    /// <summary>
    /// The number of downloads for this resource. 
    /// </summary>
    /// <remarks>
    /// <pgterms:downloads rdf:datatype="http://www.w3.org/2001/XMLSchema#integer">4641</pgterms:downloads>
    /// </remarks>
    int? Downloads { get; set; }

    /// <summary>
    /// A name given to the resource.
    /// </summary>
    /// <remarks>
    /// <dcterms:title>Common Sense</dcterms:title>
    /// </remarks>
    string Title { get; set; }

    /// <summary>
    /// Credits for persons or organizations, other than members of the cast, who 
    /// have participated in the creation and/or production of the work. 
    /// The introductory term Credits: is usually generated as a display constant.
    /// </summary>
    /// <remarks>
    /// Marc508 e.g. Produced by Biblioteca Nacional Digital (http://bnd.bn.pt),
    /// </remarks>
    string? CreationProductionCreditsNote { get; set; }

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
    string Language { get; set; }

    FileTypeEnum BookType { get; set; }

    string? Imprint { get; set; }

    string? TitleAlternative { get; set; }

    string LCSH { get; set; }

    string LCCN { get; set; }

    string PGEditionInfo { get; set; }

    string PGProducedBy { get; set; }

    string BookSeries { get; set; }

    string LCC { get; set; }

    //UserReviewViewModel? Review { get; set; }

    //List<UserNoteViewModel>? Notes { get; set; }

    //BookNavigationViewModel NavigationData { get; set; }
}
