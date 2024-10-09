using System.Collections.Generic;

using Voracious.Core.Interface;
using Voracious.RDF.Model;

namespace Voracious.RDF.Interface;

public interface IResource : IResourceCore
{
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
    List<Creator> Creators { get; set; }

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
    List<FileFormat> FileFormats { get; set; }

    ////
    //// Next is all of the user-settable things
    ////
    ////[ObservableProperty]
    ////public UserReviewViewModel? review;

    ////[ObservableProperty]
    ////public List<UserNoteViewModel>? notes;

    ////[ObservableProperty]
    ////public BookNavigationViewModel? navigationData;

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
    //public List<string> Subjects { get; set; } = [];

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
    //public List<string> Relations { get; set; } = [];
}