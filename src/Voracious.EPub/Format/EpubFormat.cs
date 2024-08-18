using Voracious.EPub.NAV;
using Voracious.EPub.NCX;
using Voracious.EPub.OCF;
using Voracious.EPub.OPF;

namespace Voracious.EPub.Format;

public class EpubFormat
{
    public EpubFormatPaths Paths { get; internal set; } = new EpubFormatPaths();

    public OcfDocument Ocf { get; internal set; }
    public OpfDocument Opf { get; internal set; }
    public NcxDocument Ncx { get; internal set; }
    public NavDocument Nav { get; internal set; }
}
