using System;
using System.Collections.Generic;

using Voracious.RDF.Enum;
using Voracious.RDF.Interface;

namespace Voracious.RDF.Model;

public class Resource : IResource
{
    #region observable properties
    /// <summary>
    /// The primary key
    /// </summary>
    public string About { get; set; } = string.Empty;

    /// <summary>
    /// An alternative name for the resource.
    /// </summary>
    public string? TitleAlternative { get; set; }

    /// <summary>
    /// The book Series
    /// </summary>
    public string BookSeries { get; set; }

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
    public FileTypeEnum BookType { get; set; }

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
    /// Examples of a contributor include a person, an organization, or
    /// a service.Typically, the name of a contributor should be used to 
    /// indicate the entity. XMP addition: XMP usage is a list of contributors. 
    /// These contributors should not include those listed in dc:creator.
    /// </remarks>
    public List<ICreator> Contributors { get; set; } = [];

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
    public List<string> Coverages { get; set; } = [];

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
    /// An entity primarily responsible for making the resource.
    /// <para>
    /// dc:creator
    /// </para>
    /// <para>
    /// Ordered array of ProperName
    /// </para>
    /// </summary>
    /// <remarks>
    /// Examples of a creator include a person, an organization, or a service.
    /// Typically, the name of a creator should be used to indicate the entity.  
    /// </remarks>
    public List<ICreator> Creators { get; set; } = [];

    /// <summary>
    /// An account of the resource.
    /// <para>
    /// dc:description
    /// </para>
    /// <para>
    /// Language Alternative
    /// </para>
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The number of times the book was downloaded
    /// </summary>
    public int? Downloads { get; set; }

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
    public string Formats { get; set; } = string.Empty;

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
    public List<Identifier> Identifiers { get; set; } = [];

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
    public string Title { get; set; } = string.Empty;

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
    public List<string> Subjects { get; set; } = [];

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
    public string? Publisher { get; set; }

    /// </summary>
    /// <summary>
    /// Date of formal issuance of the resource.
    /// </summary>
    /// <remarks>
    /// <dcterms:issued rdf:datatype="http://www.w3.org/2001/XMLSchema#date">1994-07-01</dcterms:issued>
    /// </remarks>
    public DateTime Issued { get; set; } = DateTime.Now;

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
    public string Sources { get; set; } = string.Empty;

    /// <summary>
    /// A language of the resource.
    /// <para>
    /// dc:language
    /// </para>
    /// </summary>
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// The set of conceptual resources specified by the Library of Congress
    /// Classification.
    /// </summary>
    public string LCC { get; set; }

    /// <summary>
    /// A Library of Congress catalog control number is a unique identification 
    /// number that the Library of Congress assigns to the catalog record 
    /// created for each book in its cataloged collections.
    /// </summary>
    public string LCCN { get; set; } = string.Empty;

    /// <summary>
    /// Library of Congress Subject Headings (LCSH) has been actively maintained 
    /// since 1898 to catalog materials held at the Library of Congress. 
    /// </summary>
    public string LCSH { get; set; } = string.Empty;

    /// <summary>
    /// A legal document giving official permission to do something with 
    /// the resource.
    /// </summary>
    /// <remarks>
    /// <dcterms:license rdf:resource="license"/>
    /// </remarks>
    public string? License { get; set; } = "license";

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
    public List<string> Relations { get; set; } = [];

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
    public string? Rights { get; set; } = string.Empty;

    /// <summary>
    /// marc260: Houston: Advantage International, The PaperLess Readers Club, 1992
    /// </summary>
    public string? Imprint { get; set; }

    /// <summary>
    /// Project Gutenberg Edition information
    /// </summary>
    public string PGEditionInfo { get; set; }

    /// <summary>
    /// Project Gutenberg Produced By
    /// </summary>
    public string PGProducedBy { get; set; }
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
    public List<Creator> People { get; set; } = [];

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
