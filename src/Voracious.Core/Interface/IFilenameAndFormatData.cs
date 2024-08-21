namespace Voracious.Core.Interface;

public interface IFilenameAndFormatData
{
    // Book can't be the primary key because there are duplicates. Use a synthasized Id
    // which will be maintained by the database.
    int Id { get; set; }

    string FileName { get; set; }

    string FileType { get; set; }

    string LastModified { get; set; }

    string BookId { get; set; }

    int Extent { get; set; }

    string MimeType { get; set; }
}
