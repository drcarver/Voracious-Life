using Voracious.Core.Enum;

namespace Voracious.Core.Interface;

/// <summary>
/// Person can be used for Author, Illustrator, Editor, Translator, etc.
/// </summary>
public interface IPerson
{
    string About { get; set; }

    string? Name { get; set; }

    /// <summary>
    /// Aliases is stored as a series of + separated names
    /// So it could be "john Jones" or "john jones+samantha sams"
    /// Never fiddle with the value directly! Use the AddAlias to add each alias.
    /// </summary>
    string? Aliases { get; set; }

    string? FileAs { get; set; }

    int? BirthDate { get; set; }

    int? DeathDate { get; set; }

    string? Webpage { get; set; }

    /// <summary>
    /// e.g. aut=author ill=illustator 
    /// from www.loc.gov/vocabulary/relators.html
    /// </summary>
    RelatorEnum? Relator { get; set; }
}
