using System.Collections.Generic;

namespace Voracious.Core.Interface;

public interface IGetSearchArea
{
    IList<string> GetSearchArea(string inputArea);
}
