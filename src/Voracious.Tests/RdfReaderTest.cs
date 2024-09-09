using Microsoft.Extensions.Logging;

using Voracious.Database;

namespace Voracious.Tests;

[TestClass]
public class RdfReaderTests()
{
    [TestMethod]
    public async Task RdfReaderCreateTest()
    {
        ILoggerFactory ilf = new LoggerFactory();
        CatalogDataContext gcdc = new CatalogDataContext();
        var rdf = new RdfReader(ilf, gcdc);
        await rdf.UpdateCatalogAsync();
    }
}
