using System;
using System.Collections.Generic;

using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.Core.Enum;
using Voracious.Core.Interface;

namespace Voracious.Core.ViewModel;

public partial class FilenameAndFormatDataViewModel : ObservableObject, IFilenameAndFormatData
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    private string fileName = "";

    [ObservableProperty]
    private string fileType = "";

    [ObservableProperty]
    private DateTime? lastModified = DateTime.Now;

    [ObservableProperty]
    private FileStatusEnum currentFileStatus = FileStatusEnum.Unknown;

    [ObservableProperty]
    private DateTime downloadDate = DateTime.Now;

    [ObservableProperty]
    private IResource resource;

    [ObservableProperty]
    private int extent = -1;

    [ObservableProperty]
    private string mimeType = "";
}
