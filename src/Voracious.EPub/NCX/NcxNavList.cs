using System.Collections.Generic;

namespace Voracious.EPub.NCX;

public class NcxNavList
{
    public string Id { get; set; }
    public string Class { get; set; }
    public string Label { get; set; }
    public List<NcxNavTarget> NavTargets { get; set; } = new List<NcxNavTarget>();
}
