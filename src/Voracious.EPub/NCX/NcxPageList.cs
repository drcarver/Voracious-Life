using System.Collections.Generic;

namespace Voracious.EPub.NCX;

public class NcxPageList
{
    public NcxNavInfo NavInfo { get; internal set; }

    public IList<NcxPageTarget> PageTargets { get; internal set; } = new List<NcxPageTarget>();
}
