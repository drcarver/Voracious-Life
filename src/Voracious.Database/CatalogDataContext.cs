using System.Reflection;

using Microsoft.EntityFrameworkCore;

using Voracious.Database.Configuration;
using Voracious.RDF.Model;

namespace Voracious.Database;

public class CatalogDataContext : DbContext
{
    /// <summary>
    /// Constructor
    /// </summary>
    public CatalogDataContext()
    {
    }

    /// <summary>
    /// Read all model configurations from separate files
    /// </summary>
    /// <param name="modelBuilder">The Model Builder from the data context</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(ResourceEntityTypeConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Voracious");
        Directory.CreateDirectory(folder);
        Directory.SetCurrentDirectory(folder);

        optionsBuilder.UseSqlite("Data Source=CardCatalog.db", options =>
        {
            options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
        });
        base.OnConfiguring(optionsBuilder);
    }

    //public DbSet<BookNavigation> BookNavigations { get; set; }
    //public DbSet<FilenameAndFormatDataModel> Files { get; set; }
    public DbSet<Creator> Creators { get; set; }
    public DbSet<Resource> Resources { get; set; }
    //public DbSet<UserNote> UserNotes { get; set; }
    //public DbSet<UserReview> UserReviews { get; set; }
}
