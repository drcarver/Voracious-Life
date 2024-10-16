using System.Threading.Tasks;

using Voracious.Control.ViewModel;
using Voracious.Life.Model;

namespace Voracious.Life.Interface;

public interface ISimpleBookHandler
{
    Task DisplayBook(ResourceViewModel book, BookLocation location);
}
