using Voracious.Core.Interface;
using Voracious.RDF.Model;

namespace Voracious.RDF.Interface;

public interface IBookNavigation : IBookNavigationCore
{
    Resource Resource { get; set; }
}
