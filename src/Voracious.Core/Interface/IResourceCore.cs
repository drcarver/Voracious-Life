using System;
using System.Collections.Generic;

using Voracious.Core.Enum;
using Voracious.Core.Model;

namespace Voracious.Core.Interface;

/// <summary>
/// One Gutenberg record for a book (not all data is saved)
/// </summary>
public interface IResourceCore
{
    /// <summary>
    /// The primary key
    /// </summary>
    string About { get; set; }

    /// <summary>
    /// The title or creator string to file the resource
    /// under
    /// </summary>
    string? FileAs { get; set; }

    /// <summary>
    /// The File Status of the epub file
    /// </summary>
    FileStatusEnum FileStatus { get; set; }

    /// <summary>
    /// An alternative name for the resource.
    /// </summary>
    string? TitleAlternative { get; set; }

    /// <summary>
    /// The book Series
    /// </summary>
    string BookSeries { get; set; }

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
    FileTypeEnum? BookType { get; set; }

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
    //List<string> Coverages { get; set; }

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
    /// An account of the resource.
    /// <para>
    /// dc:description
    /// </para>
    /// <para>
    /// Language Alternative
    /// </para>
    /// </summary>
    string? Description { get; set; }

    /// <summary>
    /// The number of times the book was downloaded
    /// </summary>
    int? Downloads { get; set; }

    /// <summary>
    /// An unambiguous reference to the resource within a given context.
    /// <para>
    /// dc:identifier
    /// </para>
    /// <para>
    /// Text
    /// </para>
    /// </summary>
    /// <remarks>
    /// Recommended best practice is to identify the resource by
    /// means of a string conforming to a formal identification system.
    /// </remarks>
    //List<Identifier> Identifiers { get; set; }

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
    string Title { get; set; }

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
    //List<string> Subjects { get; set; }

    /// <summary>
    /// An entity responsible for making the resource available.
    /// <para>
    /// dc:publisher
    /// </para>
    /// </summary>
    /// <remarks>
    /// Examples of a publisher include a person, an organization, or a
    /// service. Typically, the name of a publisher should be used to indicate 
    /// the entity.
    /// </remarks>
    string? Publisher { get; set; }

    /// </summary>
    /// <summary>
    /// Date of formal issuance of the resource.
    /// </summary>
    /// <remarks>
    /// <dcterms:issued rdf:datatype="http://www.w3.org/2001/XMLSchema#date">1994-07-01</dcterms:issued>
    /// </remarks>
    DateTime Issued { get; set; }

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
    string Sources { get; set; }

    /// <summary>
    /// A language of the resource.
    /// <para>
    /// dc:language
    /// </para>
    /// </summary>
    string Language { get; set; }

    /// <summary>
    /// The set of conceptual resources specified by the Library of Congress
    /// Classification.
    /// </summary>
    string LCC { get; set; }

    /// <summary>
    /// A Library of Congress catalog control number is a unique identification 
    /// number that the Library of Congress assigns to the catalog record 
    /// created for each book in its cataloged collections.
    /// </summary>
    string LCCN { get; set; }

    /// <summary>
    /// Library of Congress Subject Headings (LCSH) has been actively maintained 
    /// since 1898 to catalog materials held at the Library of Congress. 
    /// </summary>
    string LCSH { get; set; }

    /// <summary>
    /// A legal document giving official permission to do something with 
    /// the resource.
    /// </summary>
    /// <remarks>
    /// <dcterms:license rdf:resource="license"/>
    /// </remarks>
    string? License { get; set; }

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
    //List<string> Relations { get; set; }

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
    string? Rights { get; set; }

    /// <summary>
    /// marc260: Houston: Advantage International, The PaperLess Readers Club, 1992
    /// </summary>
    string? Imprint { get; set; }

    /// <summary>
    /// Project Gutenberg Edition information
    /// </summary>
    string PGEditionInfo { get; set; }

    /// <summary>
    /// Project Gutenberg Produced By
    /// </summary>
    string PGProducedBy { get; set; }

    //UserReviewViewModel? Review { get; set; }

    //List<UserNoteViewModel>? Notes { get; set; }

    //BookNavigationViewModel NavigationData { get; set; }
}
