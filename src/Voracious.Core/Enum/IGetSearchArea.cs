using System.Collections.Generic;

namespace Voracious.Core.Enum;

public interface IGetSearchArea
{
    IList<string> GetSearchArea(string inputArea);
}
