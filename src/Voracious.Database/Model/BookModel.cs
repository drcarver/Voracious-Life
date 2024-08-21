using System.Collections.ObjectModel;

using Voracious.Core.Enum;
using Voracious.Core.Interface;

namespace Voracious.Database.Model;

public class BookModel : IBook
{
    public string? BookId { get; set; }
    public string? BookSource { get; set; }
    public FileTypeEnum BookType { get; set; }
    public string? Description { get; set; }
    public string? Imprint { get; set; }
    public DateTime Issued { get; set; }
    public string? Title { get; set; }
    public string? TitleAlternative { get; set; }
    public ObservableCollection<IPerson> People { get; set; }
    public ObservableCollection<IFilenameAndFormatData> Files { get; set; }
    public string Language { get; set; }
    public string LCSH { get; set; }
    public string LCCN { get; set; }
    public string PGEditionInfo { get; set; }
    public string PGProducedBy { get; set; }
    public string PGNotes { get; set; }
    public string BookSeries { get; set; }
    public string LCC { get; set; }
    public long DenormDownloadDate { get; set; }
    public string DenormPrimaryAuthor { get; set; }
    public IUserReview Review { get; set; }
    public IBookNote Notes { get; set; }
    public IDownloadData DownloadData { get; set; }
    public IBookNavigation NavigationData { get; set; }
}
