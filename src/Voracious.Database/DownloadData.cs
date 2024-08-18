using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Voracious.Database;

/// <summary>
/// Info for every book that's been downloaded. This includes information on
/// how much of the book has been read, and its state. This gets adds for 
/// every single book that's downloaded or is known. DownloadData is per-computer and doesn't
/// get added to a bookmark file.
/// </summary>
public partial class DownloadData : INotifyPropertyChanged, INotifyPropertyChanging
{
    private int id;
    private string bookId = "";
    private string filePath = "";
    private string fileName = "";
    private FileStatusEnum currFileStatus = FileStatusEnum.Unknown;
    private DateTimeOffset downloadDate = DateTimeOffset.Now;
    public int Id { get => id; set { if (id != value) { NotifyPropertyChanging(); id = value; NotifyPropertyChanged(); } } }

    public string BookId { get => bookId; set { if (bookId != value) { NotifyPropertyChanging(); bookId = value; NotifyPropertyChanged(); } } }
    /// <summary>
    /// Path to the folder containing the file but not the FileName itself. See also FullFilePath.
    /// </summary>
    public string FilePath { get => filePath; set { if (filePath != value) { NotifyPropertyChanging(); filePath = value; NotifyPropertyChanged(); } } }

    /// <summary>
    /// Just the name of the file
    /// </summary>
    public string FileName { get => fileName; set { if (fileName != value) { NotifyPropertyChanging(); fileName = value; NotifyPropertyChanged(); } } }

    /// <summary>
    /// FilePath + FileName combined
    /// </summary>
    public string FullFilePath { get { return $"{FilePath}\\{FileName}"; } }
    public FileStatusEnum CurrFileStatus { get => currFileStatus; set { if (currFileStatus != value) { NotifyPropertyChanging(); currFileStatus = value; NotifyPropertyChanged(); } } }
    public DateTimeOffset DownloadDate { get => downloadDate; set { if (downloadDate != value) { NotifyPropertyChanging(); downloadDate = value; NotifyPropertyChanged(); } } }

    public event PropertyChangedEventHandler PropertyChanged;
    public event PropertyChangingEventHandler PropertyChanging;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    private void NotifyPropertyChanging([CallerMemberName] string propertyName = "")
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }
}
