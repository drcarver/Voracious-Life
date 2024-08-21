using System.Formats.Tar;
using System.IO.Compression;

using Voracious.Core.Extension;

namespace Voracious.Tests;

[TestClass]
public class ZipTests
{
    [TestMethod]
    public void OpenZipFile()
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
        string fpath= $@"{folder}\Voracious\";
        Directory.CreateDirectory(fpath);
        string fullpath = $@"{fpath}\rdf-files.tar.zip";
        int epubCount = 0;
        using (var zipArchive = ZipFile.OpenRead(fullpath))
        {
            string tarpath = $@"{fpath}\rdf-files.tar";
            zipArchive.ExtractToDirectory(fpath, true);
            using (StreamReader sr = new StreamReader(File.OpenRead($@"{fpath}\rdf-files.tar")))
            {
                TarReader tarReader = new TarReader(sr.BaseStream);
                TarEntry entry = null;
                while ((entry = tarReader.GetNextEntry()) != null)
                {
                    if (entry.Name.ToLower().Contains("epub"))
                    {
                        string rdf = Utility.ReadAllText(entry.DataStream);
                    }
                    epubCount++;
                }
            }
        }
        Assert.AreEqual(epubCount, 74114, "Epub counts are different");
    }
}