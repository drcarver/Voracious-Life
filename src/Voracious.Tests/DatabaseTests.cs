using Microsoft.EntityFrameworkCore;

using Voracious.Database;

namespace Voracious.Tests;

[TestClass]
public class DatabaseTests
{
    [TestMethod]
    public void CreateDataBaseTest()
    {
        var db = new CatalogDataContext();
        db.Database.EnsureCreated();
        Assert.AreEqual(0, db.Resources.Count());

        // Now delete the database
        db.Database.EnsureDeleted();
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
        string fpath = $@"{folder}\Voracious\";
        Assert.IsFalse(File.Exists($@"{fpath}\ebook.db"));
    }

    [TestMethod]
    public void DatabaseMigrationTest()
    {
        var db = new CatalogDataContext();
        db.Database.Migrate();
        Assert.AreEqual(0, db.Resources.Count());

        // Now delete the database
        db.Database.EnsureDeleted();
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
        string fpath = $@"{folder}\Voracious\";
        Assert.IsFalse(File.Exists($@"{fpath}\ebook.db"));
    }

    [TestMethod]
    public void DatabaseDeleteTest()
    {
        var db = new CatalogDataContext();

        // Now delete the database
        db.Database.EnsureDeleted();
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
        string fpath = $@"{folder}\Voracious\";
        Assert.IsFalse(File.Exists($@"{fpath}\ebook.db"));
    }
}
