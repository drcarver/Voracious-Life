using System;

using Voracious.Core.Enum;

namespace Voracious.Core.Interface;

public interface IFilenameAndFormatData
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
