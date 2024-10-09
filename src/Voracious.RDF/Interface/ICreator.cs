using System.Collections.Generic;

using Voracious.Core.Interface;
using Voracious.RDF.Model;

namespace Voracious.RDF.Interface;

public interface ICreator : ICreatorCore
{
    /// <summary>
    /// People include authors, illustrators, etc.
    /// </summary>
    List<Resource> Resources { get; set; }
}