using System.Collections.Generic;

using Voracious.Core.Enum;

namespace Voracious.Core.Interface;

/// <summary>
/// Person can be used for Author, Illustrator, Editor, 
/// Translator, etc.
/// </summary>
public interface ICreator : ICreator
{
    /// <summary>
    /// People include authors, illustrators, etc.
    /// </summary>
    List<Resource> Resources { get; set; }
}
