using System.Collections.Generic;

namespace Voracious.EPub.NCX;

public class NcxPageList
{
    public NcxNavInfo NavInfo { get; set; }

    public List<NcxPageTarget> PageTargets { get; set; } = new List<NcxPageTarget>();
}
