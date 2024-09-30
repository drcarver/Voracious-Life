using System.Formats.Tar;
using System.IO.Compression;

using Voracious.RDF.Extension;

namespace Voracious.Tests;

[TestClass]
public class ZipTests
{
    [TestMethod]
    public void SetCurrentDirectoryTest()
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
        string fpath = $@"{folder}\Voracious\";
        Directory.CreateDirectory(fpath);
        Directory.SetCurrentDirectory(fpath);

        Assert.IsTrue(File.Exists("rdf-files.tar.zip"));
    }

    [TestMethod]
    public void OpenZipFileTest()
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
        string fpath = $@"{folder}\Voracious\";
        Directory.CreateDirectory(fpath);
        Directory.SetCurrentDirectory(fpath);

        string fname = $"rdf-files.tar.zip";
        var zipArchive = ZipFile.OpenRead(fname);
        Assert.AreEqual(zipArchive.Entries[0].Name, "rdf-files.tar");
        zipArchive.Dispose();
    }

    [TestMethod]
    public void CountEntriesInTarFileTest()
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
        string fpath = $@"{folder}\Voracious\";
        Directory.CreateDirectory(fpath);
        Directory.SetCurrentDirectory(fpath);

        string fname = $"rdf-files.tar.zip";
        int epubCount = 0;
        using (var zipArchive = ZipFile.OpenRead(fname))
        {
            string tarpath = $@"rdf-files.tar";
            zipArchive.ExtractToDirectory(fpath, true);
            using (StreamReader sr = new StreamReader(File.OpenRead($@"{fpath}\rdf-files.tar")))
            {
                TarReader tarReader = new(sr.BaseStream);
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