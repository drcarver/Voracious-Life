using System;

using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.Core.Enum;
using Voracious.Core.Interface;

namespace Voracious.Core.ViewModel;

/// <summary>
/// Info for every book that's been downloaded. This includes information on
/// how much of the book has been read, and its state. This gets adds for 
/// every single book that's downloaded or is known. DownloadData is per-computer and doesn't
/// get added to a bookmark file.
/// </summary>
public partial class DownloadDataViewModel : ObservableObject, IDownloadData
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private string bookId = "";

    [ObservableProperty]
    private string filePath = "";

    [ObservableProperty]
    private string fileName = "";

    [ObservableProperty]
    private FileStatusEnum currFileStatus = FileStatusEnum.Unknown;

    [ObservableProperty]
    private DateTimeOffset downloadDate = DateTimeOffset.Now;

    /// <summary>
    /// FilePath + FileName combined
    /// </summary>
    public string FullFilePath { get { return $"{FilePath}\\{FileName}"; } }
}
