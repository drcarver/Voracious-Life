using Voracious.EPub.NAV;
using Voracious.EPub.NCX;
using Voracious.EPub.OCF;
using Voracious.EPub.OPF;

namespace Voracious.EPub.Format;

public class EpubFormat
{
    public EpubFormatPaths Paths { get; set; } = new EpubFormatPaths();

    public OcfDocument Ocf { get; set; }
    public OpfDocument Opf { get; set; }
    public NcxDocument Ncx { get; set; }
    public NavDocument Nav { get; set; }
}
