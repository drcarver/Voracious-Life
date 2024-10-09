using CommunityToolkit.Mvvm.ComponentModel;

using Voracious.Core.Enum;
using Voracious.Core.Interface;
using Voracious.Core.Model;

namespace Voracious.Life.ViewModel;

public partial class FileFormatViewModel : ObservableObject, IFileFormatCore
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
    private ResourceCore resource;

    [ObservableProperty]
    private int extent = -1;

    [ObservableProperty]
    private string mimeType = "";
}
