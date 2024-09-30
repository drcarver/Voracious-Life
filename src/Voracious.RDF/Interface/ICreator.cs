using System.Collections.Generic;

using Voracious.RDF.Enum;
using Voracious.RDF.Model;

namespace Voracious.RDF.Interface;

/// <summary>
/// Person can be used for Author, Illustrator, Editor, Translator, etc.
/// </summary>
public interface ICreator
{
    /// <summary>
    /// The primary key
    /// </summary>
    string About { get; set; }

    /// <summary>
    /// The alternate script for the creator
    /// </summary>
    string AlternateScript { get; set; }

    /// <summary>
    /// The name of the person
    /// </summary>
    string? Name { get; set; }

    /// <summary>
    /// The person's alias
    /// </summary>
    string? Aliases { get; set; }

    /// <summary>
    /// The date of birth
    /// </summary>
    int? BirthDate { get; set; }

    /// <summary>
    /// The date of death
    /// </summary>
    int? DeathDate { get; set; }

    /// <summary>
    /// THe author's web page
    /// </summary>
    string? Webpage { get; set; }

    /// <summary>
    /// The sortable version of the author
    /// </summary>
    string? FileAs { get; set; }

    /// <summary>
    /// The relator's for the book (creator, illustrator, etc..)
    /// </summary>
    RelatorEnum? Role { get; set; }

    /// <summary>
    /// People include authors, illustrators, etc.
    /// </summary>
    List<Resource> Resources { get; set; }

    /// <summary>
    /// Add the alias for the person
    /// </summary>
    /// <param name="value"></param>
    void AddAlias(string value);

    /// <summary>
    /// Get the Importance of a Creator to the resource
    /// </summary>
    /// <returns>The persons importance</returns>
    int GetImportance();

    /// <summary>
    /// unknown is e.g. book http://www.gutenberg.org/ebooks/2822
    /// where Daniel Defoe is somehow part of this book, we just
    /// don't know how.  In the text, the book is attributed to Defoe.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    RelatorEnum ToRelator(string value);
}
