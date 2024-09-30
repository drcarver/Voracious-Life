using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using CsvHelper;

namespace Voracious.RDF;

/// <summary>
/// <a href="https://www.iana.org/assignments/media-types/media-types.xhtml">Internet assigned number authority</a>
/// </summary>
public class IAnnaReader
{
    public void ReadIAnna()
    {
        using (var reader = new StreamReader("path\\to\\file.csv"))
        {
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                //var records = csv.GetRecords<Foo>();
            }
        }
    }
}
