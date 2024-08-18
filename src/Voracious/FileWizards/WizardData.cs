using SimpleEpubReader.Database;

namespace Voracious.FileWizards;

/// <summary>
/// Used to get data about ebooks. Fill in the FilePath and then call the appropriate
/// Wizard.GetData(data) routine to learn info about the book. Most critically, will fill
/// in the BookId that should match the BookId in the catalog database.
/// </summary>
public class WizardData
{
    public string FilePath { get; set; }
    public string FileName { get; set; }

    public void ClearGetData()
    {
        BookId = null;
        BD = null;
    }
    public string BookId { get; set; }
    public BookData BD { get; set; }
}
