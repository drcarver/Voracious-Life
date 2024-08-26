using System.Collections.Generic;

namespace Voracious.EPub.OPF;

public class OpfGuide
{
    public IList<OpfGuideReference> References { get; internal set; } = new List<OpfGuideReference>();
}
