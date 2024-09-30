using System;

using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.RDF.Enum;
using Voracious.RDF.Interface;

namespace Voracious.Core.ViewModel;

public partial class FileFormatViewModel : ObservableObject, IFileFormat
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
