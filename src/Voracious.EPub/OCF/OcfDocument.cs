using System.Collections.Generic;
using System.Linq;

using Voracious.EPub.Format;

namespace Voracious.EPub.OCF;

public class OcfDocument
{
    public List<OcfRootFile> RootFiles { get; set; } = new List<OcfRootFile>();

    private OcfRootFile rootFile;
    public string RootFilePath => rootFile?.FullPath ?? (rootFile = RootFiles.FirstOrDefault(e => e.MediaType == Constants.OcfMediaType))?.FullPath;
}
