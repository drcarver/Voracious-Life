using System;
using System.Collections.Generic;
using System.Linq;

using Voracious.Core.Enum;
using Voracious.Core.Model;
using Voracious.RDF.Interface;

namespace Voracious.RDF.Model;

public class Resource : ResourceCore, IResource
{
    #region navigation properties and commands
    /// <summary>
    /// An entity primarily responsible for making the resource
    /// </summary>
    /// <remarks>
    /// A second property with the same name as this property has been declared in 
    /// the dcterms: namespace. See the Introduction to the document DCMI Meta-data 
    /// Terms for an explanation.
    /// </remarks>
    public List<Creator> Creators { get; set; } = [];

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
    public List<FileFormat> FileFormats { get; set; } = [];

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
    #endregion

    #region cleanup methods
    /// <summary>
    /// Create a file as string for the resource
    /// </summary>
    public void CleanupFileResourceAs()
    {
        FileAs = Title.ToUpper();
        Creator creator = Creators.OrderBy(c => c.GetImportance()).FirstOrDefault();
        
        if (creator != null) 
        {
            FileAs += $" {creator.Role} {creator.FileAs}";
        }
        if (FileAs != null)
        {
            if (FileAs.StartsWith("A "))
            {
                FileAs = FileAs.Substring(2);
            }
            if (FileAs.StartsWith("AN "))
            {
                FileAs = FileAs.Substring(3);
            }
            if (FileAs.StartsWith("THE "))
            {
                FileAs = FileAs.Substring(4);
            }
        }
    }
    #endregion
}
