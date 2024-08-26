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
            string fpath = $@"{folder}\Voracious\Mylibrary\More_Than_Honor.epub";
            var book = new EpubReader().Read(fpath);

            Assert.AreEqual(book.Title, "More Than Honor");
        }
    }
}
