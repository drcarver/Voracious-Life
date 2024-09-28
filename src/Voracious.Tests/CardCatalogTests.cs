using Microsoft.Extensions.Logging;

using Voracious.Database;

namespace Voracious.Tests;

[TestClass]
public class CardCatalogTests()
{
    [TestMethod]
    public async Task CardCatalogCreateTest()
    {
        ILoggerFactory ilf = new LoggerFactory();
        CatalogDataContext gcdc = new CatalogDataContext();
        var rdf = new CardCatalog(ilf, gcdc);
        await rdf.UpdateCatalogAsync();
    }
}
