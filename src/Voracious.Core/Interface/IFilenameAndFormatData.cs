using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.Core.Enum;
using Voracious.Core.ViewModel;

namespace Voracious.Core.Interface;

public interface IFilenameAndFormatData
{
    int Id { get; set; }

    string FileName { get; set; }

    string FileType { get; set; }

    FileStatusEnum CurrentFileStatus { get; set; }

    DateTimeOffset DownloadDate { get; set; }

    string LastModified { get; set; }

    int Extent { get; set; }

    string MimeType { get; set; }
}
