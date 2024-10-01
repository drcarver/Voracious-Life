using Microsoft.Extensions.Logging;

using Voracious.Database;
using Voracious.EPub;

namespace Voracious.Tests
{
    [TestClass]
    public class EPubTests
    {
        [TestMethod]
        public void EPubReadTest()
        {
            string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
            string fpath = $@"{folder}\Voracious\Import\More_Than_Honor.epub";
            var book = new EpubReader().Read(fpath);

            Assert.AreEqual(book.Title, "More Than Honor");
        }

        [TestMethod]
        public void ImportEPubTest()
        {
            ILoggerFactory ilf = new LoggerFactory();
            CatalogDataContext gcdc = new CatalogDataContext();
            var rdf = new CardCatalog(ilf, gcdc);
            var epubs = rdf.ImportEpubFolder();
            Assert.AreEqual(113, epubs.Count());
        }
    }
}
