using System;
using System.Collections.Generic;

using Voracious.Core.Enum;
using Voracious.Core.Interface;

namespace Voracious.RDF.Model;

public class FileFormat : IFileFormatCore
{
    /// <summary>
    /// The primary key of the file
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The filename of the file
    /// </summary>
    public string FileName { get; set; } = "";

    /// <summary>
    /// The file type 
    /// </summary>
    public string FileType { get; set; } = "";

    /// <summary>
    /// The last modified date time
    /// </summary>
    public DateTime? LastModified { get; set; } = DateTime.Now;

    /// <summary>
    /// The file status enum
    /// </summary>
    public FileStatusEnum CurrentFileStatus { get; set; } = FileStatusEnum.Unknown;

    /// <summary>
    /// The date and time the file was downloaded 
    /// </summary>
    public DateTime DownloadDate { get; set; } = DateTime.Now;

    /// <summary>
    /// The resource the file is associated with
    /// </summary>
    public Resource Resource { get; set; }

    /// <summary>
    /// The file extent
    /// </summary>
    public int Extent { get; set; } = -1;

    /// <summary>
    /// The mime type of the file
    /// </summary>
    public string MimeType { get; set; } = "";

    /// <summary>
    /// The files are the variants of an ebook (plus ancillary stuff like title 
    /// pages). Given a list of possible files, return an ordered list of the 
    /// most appropriate files for the book, filtering to remove extra ones. 
    /// For examples, if there's an epub with images and an epub without images, 
    /// only include the epub with images.
    /// </summary>
    /// <param name="start"></param>
    /// <returns></returns>
    public List<FileFormat> GetProcessedFileList(List<FileFormat> start)
    {
        // For example: if there's an epub with images, don't include the epub without images.
        // If there's a high-res cover, don't include a low-res cover.
        // If there are any text files at all, don't include HTML.
        // Assumes that the original list is pretty random (which seems to be the case
        // for the XML data) and that the order doesn't matter. This is probably OK
        // because I don't expect, e.g., multiple conflicting files like two different
        // text versions.
        // FAIL: actually, the audio books includes a bazillion OGG etc files.

        var sortedlist = new List<FileFormat>();
        var retval = new List<FileFormat>();

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

    /// <summary>
    /// Get the File Type
    /// </summary>
    /// <returns>The processed file enum</returns>
    public ProcessedFileEnum GetFileType()
    {
        switch (MimeType)
        {
            case "application/epub+zip":
                //FAIL: they have two different patterns for epubs with images.
                return FileName.Contains(".images") || FileName.Contains("-images.epub") ? ProcessedFileEnum.EPub : ProcessedFileEnum.EPubNoImages;

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
                if (string.IsNullOrEmpty(FileName))
                { 
                    return ProcessedFileEnum.CoverSmall; 
                }
                if (FileName.Contains("cover.small"))
                {
                    return ProcessedFileEnum.CoverSmall;
                }
                if (FileName.Contains("cover.medium"))
                {
                    return ProcessedFileEnum.CoverMedium;
                }
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

    /// <summary>
    /// Is the file a known MimeType
    /// </summary>
    public bool IsKnownMimeType
    {
        get
        {
            switch (MimeType)
            {
                case "application/epub+zip":
                case "application/msword": // word doc e.g. 10681 and 80+ others
                case "application/octet-stream": // seemingly obsolete -- used for old books only?
                case "application/pdf":
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

    /// <summary>
    /// Return the file type as a string
    /// </summary>
    /// <returns>The file type</returns>
    public string FileTypeAsString()
    {
        switch (GetFileType())
        {
            case ProcessedFileEnum.CoverMedium:
                return "Image file (book cover)";
            case ProcessedFileEnum.CoverSmall:
                return "Image file (book cover)";
            case ProcessedFileEnum.EPub:
                return "EPUB";
            case ProcessedFileEnum.EPubNoImages:
                return "EPUB (no images)";
            case ProcessedFileEnum.Html:
                return "HTML web file";
            case ProcessedFileEnum.MobiPocket:
                return "Kindle (MobiPocket)";
            case ProcessedFileEnum.PDF:
                return "PDF";
            case ProcessedFileEnum.PS:
                return "PostScript";
            case ProcessedFileEnum.RDF:
                return "RDF Index File";
            case ProcessedFileEnum.Tex:
                return "Tex pre-press file";
            case ProcessedFileEnum.Text:
                return "Plain text file";
            case ProcessedFileEnum.TextNotUtf8:
                return "Plain text file";
            case ProcessedFileEnum.Unknown:
            default:
                return $"Other file type ({MimeType})";
        }
    }
}
