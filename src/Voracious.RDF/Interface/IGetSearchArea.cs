using System.Collections.Generic;

namespace Voracious.RDF.Interface;

public interface IGetSearchArea
{
    List<string> GetSearchArea(string inputArea);
}
