using System.Collections.Generic;
using System.Xml.Linq;

using Voracious.Core.Enum;
using Voracious.Core.Interface;

namespace Voracious.Core.Model;

public class CreatorCore : ICreatorCore
{
    /// <summary>
    /// The primary key
    /// </summary>
    public string About { get; set; }

    /// <summary>
    /// The alternate script for the creator
    /// </summary>
    public string AlternateScript { get; set; } = null;

    /// <summary>
    /// The name of the person
    /// </summary>
    public string Name { get; set; } = null;

    /// <summary>
    /// The person's alias
    /// </summary>
    public string Aliases { get; set; } = null;

    /// <summary>
    /// The date of birth
    /// </summary>
    public int? BirthDate { get; set; } = null;

    /// <summary>
    /// The date of death
    /// </summary>
    public int? DeathDate { get; set; } = null;

    /// <summary>
    /// The author's web page
    /// </summary>
    public string Webpage { get; set; } = null;

    /// <summary>
    /// The sortable version of the author
    /// </summary>
    public string FileAs { get; set; } = null;

    /// <summary>
    /// The relator's for the book (creator, illustrator, etc..)
    /// </summary>
    public RelatorEnum? Role { get; set; } = null;
}
