using System.Collections.Generic;

namespace Voracious.EPub.NCX;

/// <summary>
/// DAISY’s Navigation Center Extended (NCX)
/// </summary>
public class NcxDocument
{
    public List<NcxMeta> Meta { get; set; } = new List<NcxMeta>();
    public string DocTitle { get; set; }
    public string DocAuthor { get; set; }
    public NcxNapMap NavMap { get; set; } = new NcxNapMap(); // <navMap> is a required element in NCX.
    public NcxPageList PageList { get; set; }
    public NcxNavList NavList { get; set; }
}
