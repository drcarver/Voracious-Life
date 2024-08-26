using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.Core.Enum;
using Voracious.Core.Interface;

namespace Voracious.Core.ViewModel;

public partial class FilenameAndFormatDataViewModel : ObservableObject, IFilenameAndFormatData
{
    public FilenameAndFormatDataViewModel()
    {
    }

    public FilenameAndFormatDataViewModel(FilenameAndFormatDataViewModel source)
    {
        this.Id = source.Id;
        this.BookId = source.BookId;
        this.Extent = source.Extent;
        this.FileName = source.FileName;
        this.FileType = source.FileType;
        this.LastModified = source.LastModified;
        this.MimeType = source.MimeType;
    }

    // Book can't be the primary key because there are duplicates. Use a synthesized Id
    // which will be maintained by the database.
    [ObservableProperty]
    [property: Key]
    private int id;

    [ObservableProperty]
    private string fileName = "";

    [ObservableProperty]
    private string fileType = "";

    [ObservableProperty]
    private string lastModified = "";

    [ObservableProperty]
    private string bookId = "";

    [ObservableProperty]
    private int extent = -1;

    [ObservableProperty]
    private string mimeType = "";

    /// <summary>
    /// The files are the variants of an ebook (plus ancillary stuff like title pages).
    /// Given a list of possible files, return an ordered list of the most appropriate
    /// files for the book, filtering to remove extra ones. For examples, if there's an
    /// epub with images and an epub without images, only include the epub with images.
    /// </summary>
    /// <param name="start"></param>
    /// <returns></returns>
    public static IList<FilenameAndFormatDataViewModel> GetProcessedFileList(IList<FilenameAndFormatDataViewModel> start)
    {
        // For example: if there's an epub with images, don't include the epub without images.
        // If there's a high-res cover, don't include a low-res cover.
        // If there are any text files at all, don't include HTML.
        // Assumes that the original list is pretty random (which seems to be the case
        // for the XML data) and that the order doesn't matter. This is probably OK
        // because I don't expect, e.g., multiple conflicting files like two different
        // text versions.
        // FAIL: actually, the audio books includes a bazillion OGG etc files.

        var sortedlist = new List<FilenameAndFormatDataViewModel>();
        var retval = new List<FilenameAndFormatDataViewModel>();

        foreach (var item in start) sortedlist.Add(item);
        sortedlist.Sort((a, b) => { return a.GetFileType().CompareTo(b.GetFileType()); });
        bool haveEpub = false;
        bool haveCover = false;
        bool haveHtml = false;
        bool haveText = false;

        // Step one: figure out what we've got.
        foreach (var item in sortedlist)
        {
            var itemtype = item.GetFileType();
            switch (itemtype)
            {
                case ProcessedFileEnum.CoverMedium:
                case ProcessedFileEnum.CoverSmall:
                    if (!haveCover)
                    {
                        retval.Add(item);
                        haveCover = true;
                    }
                    break;
                case ProcessedFileEnum.EPub:
                case ProcessedFileEnum.EPubNoImages:
                    if (!haveEpub)
                    {
                        retval.Add(item);
                        haveEpub = true;
                    }
                    break;
                case ProcessedFileEnum.PDF:
                case ProcessedFileEnum.PS:
                case ProcessedFileEnum.Tex:
                    // Only include the PDF/PS if we don't have an epub. Although some people
                    // might prefer a PDF, the Gutenberg reality is that they create epubs,
                    // not pdfs. The few cases with PDFs are a total anomoly.
                    if (!haveEpub)
                    {
                        retval.Add(item);
                    }
                    break;
                case ProcessedFileEnum.RDF:
                    retval.Add(item);
                    break;
                case ProcessedFileEnum.Text:
                case ProcessedFileEnum.TextNotUtf8:
                    if (!haveText)
                    {
                        retval.Add(item);
                        haveText = true;
                    }
                    break;
                case ProcessedFileEnum.Html:
                case ProcessedFileEnum.HtmlNotUtf8:
                    if (!haveHtml) // Only include HTML if we don't have epub. And only include one.
                    {
                        if (!haveEpub)
                        {
                            retval.Add(item);
                        }
                        haveHtml = true;
                    }
                    break;

                case ProcessedFileEnum.MobiPocket:
                case ProcessedFileEnum.Unknown:
                    if (!haveEpub)
                    {
                        retval.Add(item);
                    }
                    break;
            }
        }

        return retval;
    }

    public int GutenbergStyleIndexNumber
    {
        get
        {
            var id = BookId;
            var idx = id.IndexOf('/');
            if (idx < 0) return 0;
            var nstr = id.Substring(idx + 1);
            int gutIndex = 0;
            Int32.TryParse(nstr, out gutIndex);
            return gutIndex;
        }
    }

    public string FileTypeAsString()
    {
        switch (GetFileType())
        {
            case ProcessedFileEnum.CoverMedium: return "Image file (book cover)";
            case ProcessedFileEnum.CoverSmall: return "Image file (book cover)";
            case ProcessedFileEnum.EPub: return "EPUB";
            case ProcessedFileEnum.EPubNoImages: return "EPUB (no images)";
            case ProcessedFileEnum.Html: return "HTML web file";
            case ProcessedFileEnum.MobiPocket: return "Kindle (MobiPocket)";
            case ProcessedFileEnum.PDF: return "PDF";
            case ProcessedFileEnum.PS: return "PostScript";
            case ProcessedFileEnum.RDF: return "RDF Index File";
            case ProcessedFileEnum.Tex: return "Tex pre-press file";
            case ProcessedFileEnum.Text: return "Plain text file";
            case ProcessedFileEnum.TextNotUtf8: return "Plain text file";
            case ProcessedFileEnum.Unknown:
            default:
                return $"Other file type ({MimeType})";
        }
    }

    public ProcessedFileEnum GetFileType()
    {
        switch (MimeType)
        {
            case "application/epub+zip":
                //FAIL: they have two different patterns for epubs with images.
                return (FileName.Contains(".images") || FileName.Contains("-images.epub")) ? ProcessedFileEnum.EPub : ProcessedFileEnum.EPubNoImages;

            case "application/octet-stream": // seemingly obsolete -- used for old books only?
                return ProcessedFileEnum.Unknown;

            case "application/pdf": // PDF file
                return ProcessedFileEnum.PDF;

            case "application/postscript": // postscript, of course
                return ProcessedFileEnum.PS;

            case "application/prs.tex": // TEX files!
                return ProcessedFileEnum.Tex;

            case "application/rdf+xml": // the RDF file
                return ProcessedFileEnum.RDF;

            case "application/x-mobipocket-ebook": // kindle
                return ProcessedFileEnum.MobiPocket;

            case "application/zip": // HTML has two formats: /zip and /html
                return ProcessedFileEnum.Html;

            case "image/jpeg": // cover images
                if (String.IsNullOrEmpty(FileName)) return ProcessedFileEnum.CoverSmall;
                if (FileName.Contains("cover.small")) return ProcessedFileEnum.CoverSmall;
                if (FileName.Contains("cover.medium")) return ProcessedFileEnum.CoverMedium;
                return ProcessedFileEnum.CoverSmall;

            case "text/html":
            case "text/html; charset=iso-8859-1":
            case "text/html; charset=us-ascii":
                return ProcessedFileEnum.HtmlNotUtf8;

            case "text/html; charset=utf-8":
                return ProcessedFileEnum.Html;

            case "text/plain":
            case "text/plain; charset=iso-8859-1":
            case "text/plain; charset=us-ascii":
                return ProcessedFileEnum.TextNotUtf8;

            case "text/plain; charset=utf-8":
                return ProcessedFileEnum.Text;
            default:
                return ProcessedFileEnum.Unknown;
        }
    }

    public bool IsKnownMimeType
    {
        get
        {
            switch (MimeType)
            {
                case "application/epub+zip":
                case "application/msword": // word doc e.g. 10681 and 80+ others
                case "application/octet-stream": // seemingly obsolete -- used for old books only?
                case "application/pdf": // PDF file
                case "application/postscript": // postscript, of course
                case "application/prs.tei": // XML text file (about 520) -- see https://en.wikipedia.org/wiki/Text_Encoding_Initiative
                case "application/prs.tex": // TEX files!
                case "application/rdf+xml": // the RDF file
                case "application/x-mobipocket-ebook": // kindle
                case "application/x-iso9660-image": // USed by the CD and DVD projects e.g. 10802 -- about 200+
                case "application/zip": // HTML has two formats: /zip and /html
                case "audio/midi": // MIDI music files e.g. jingle bells 10535 about 2500+
                case "audio/mp4": // MP4 e.g. 19450 about 9000+
                case "audio/mpeg": // MPEG about 23000+
                case "audio/ogg": // OGG VORBIS format about 23000+
                case "audio/x-ms-wma": // Microsoft format e.g. 36567 (really, just that one)
                case "audio/x-wav": //
                case "image/gif": // cover images
                case "image/jpeg": // cover images
                case "image/png": // image
                case "image/tiff": // image
                case "text/html":
                case "text/html; charset=iso-8859-1":
                case "text/html; charset=iso-8859-2":
                case "text/html; charset=iso-8859-15":
                case "text/html; charset=us-ascii":
                case "text/html; charset=utf-8":
                case "text/html; charset=windows-1251":
                case "text/html; charset=windows-1252":
                case "text/html; charset=windows-1253":
                case "text/plain":
                case "text/plain; charset=big5": // just one, 11002
                case "text/plain; charset=ibm850": // just one, 11522
                case "text/plain; charset=iso-8859-1":
                case "text/plain; charset=iso-8859-2": // about 13
                case "text/plain; charset=iso-8859-3": // about 4
                case "text/plain; charset=iso-8859-7": // about 5
                case "text/plain; charset=iso-8859-15": // about 16
                case "text/plain; charset=us-ascii":
                case "text/plain; charset=utf-7": // about 2 both of them  7467
                case "text/plain; charset=utf-8":
                case "text/plain; charset=utf-16": // seriously? 1, 13083
                case "text/plain; charset=windows-1250": // 
                case "text/plain; charset=windows-1251": // 
                case "text/plain; charset=windows-1252": // 
                case "text/plain; charset=windows-1253": // 
                case "text/rtf": // 
                case "text/rtf; charset=iso-8859-1": // 
                case "text/rtf; charset=us-ascii": // 
                case "text/x-rst": // reStructured Text https://en.wikipedia.org/wiki/ReStructuredText
                case "text/rst; charset=us-ascii": // reStructured Text https://en.wikipedia.org/wiki/ReStructuredText
                case "text/xml":
                case "text/xml; charset=iso-8859-1":
                case "video/mpeg":
                case "video/quicktime":
                case "video/x-msvideo":
                    return true;
                default:
                    return false;
            }
        }
    }

    public override string ToString()
    {
        return this.FileName;
    }
}
