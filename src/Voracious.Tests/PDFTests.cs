using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;

using Voracious.EPub;

namespace Voracious.Tests
{
    [TestClass]
    public class PDFTests
    {
        [TestMethod]
        public void PDFTitleTest()
        {
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
            string fpath = $@"{folder}\Voracious\Import\C#_Succinctly.pdf";

            FileStream inputPDFStream = new FileStream(fpath, FileMode.Open, FileAccess.Read, FileShare.Read);
            PdfLoadedDocument loadedDocument = new PdfLoadedDocument(inputPDFStream);

            var meta = loadedDocument.DocumentInformation.XmpMetadata;
            var dc = meta.DublinCoreSchema;
            Assert.AreEqual(dc.Title.DefaultText, "C# Succinctly");
            Assert.AreEqual(dc.Contributor.Items[0], "C# Succinctly");
        }
    }
}
