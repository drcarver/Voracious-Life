using System.Collections.Generic;

namespace Voracious.EPub;

public class EpubResources
{
    public List<EpubTextFile> Html { get; set; } = new List<EpubTextFile>();
    public List<EpubTextFile> Css { get; set; } = new List<EpubTextFile>();
    public List<EpubByteFile> Images { get; set; } = new List<EpubByteFile>();
    public List<EpubByteFile> Fonts { get; set; } = new List<EpubByteFile>();
    public List<EpubFile> Other { get; set; } = new List<EpubFile>();

    /// <summary>
    /// This is a concatenation of all the resources files in 
    /// the epub: html, css, images, etc.
    /// </summary>
    public List<EpubFile> All { get; set; } = new List<EpubFile>();
}
