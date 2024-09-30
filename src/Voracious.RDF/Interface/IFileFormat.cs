using System;

using Voracious.RDF.Enum;

namespace Voracious.RDF.Interface;

public interface IFileFormat
{
    int Id { get; set; }
    string FileName { get; set; }
    string FileType { get; set; }
    FileStatusEnum CurrentFileStatus { get; set; }
    DateTime DownloadDate { get; set; }
    DateTime? LastModified { get; set; }
    int Extent { get; set; }
    string MimeType { get; set; }
}
