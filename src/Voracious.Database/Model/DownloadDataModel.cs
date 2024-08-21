using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Voracious.Core.Enum;
using Voracious.Core.Interface;

namespace Voracious.Database.Model;

public class DownloadDataModel : IDownloadData
{
    public int Id { get; set; }
    public string BookId { get; set; }
    public string FilePath { get; set; }
    public string FileName { get; set; }
    public FileStatusEnum CurrFileStatus { get; set; }
    public DateTimeOffset DownloadDate { get; set; }
    public string FullFilePath { get; }
}
