using System;

using Voracious.Core.Enum;

namespace Voracious.Core.Interface;

/// <summary>
/// Info for every book that's been downloaded. This includes information on
/// how much of the book has been read, and its state. This gets adds for 
/// every single book that's downloaded or is known. DownloadData is per-computer and doesn't
/// get added to a bookmark file.
/// </summary>
public interface IDownloadData
{
    int Id { get; set; }

    string BookId { get; set; }

    string FilePath { get; set; }

    string FileName { get; set; }

    FileStatusEnum CurrFileStatus { get; set; }

    DateTimeOffset DownloadDate { get; set; }

    /// <summary>
    /// FilePath + FileName combined
    /// </summary>
    string FullFilePath { get; }
}
