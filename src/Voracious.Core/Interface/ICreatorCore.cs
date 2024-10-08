using System.Collections.Generic;

using Voracious.Core.Enum;

namespace Voracious.Core.Interface;

/// <summary>
/// Person can be used for Author, Illustrator, Editor, 
/// Translator, etc.
/// </summary>
public interface ICreatorCore
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
}
