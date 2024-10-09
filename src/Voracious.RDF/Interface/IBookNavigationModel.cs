using Voracious.Core.Interface;
using Voracious.RDF.Model;

namespace Voracious.RDF.Interface;

public interface IBookNavigationModel : IBookNavigation
{
    ResourceModel Resource { get; set; }
}
