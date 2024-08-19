using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Voracious.Database;

public partial class BookDataContext : DbContext
{
    private ILogger logger;

    public BookDataContext(ILoggerFactory loggerFactory)
    {
        ILogger logger = loggerFactory.CreateLogger<BookDataContext>();
    }

    public const string BookSourceUser = "User-imported";
    public const string BookSourceBookMarkFile = "From-bookmark-file:";

    public static string BookDataDatabaseFilename = "BookData.db";
    public DbSet<BookDataViewModel> Books { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
        var path = folder.Path;
        folder.CreateFileAsync(BookDataDatabaseFilename, Windows.Storage.CreationCollisionOption.OpenIfExists).AsTask().Wait();
        string dbpath = Path.Combine(path, BookDataDatabaseFilename);
        options.UseSqlite($"Data Source={dbpath}");
    }

    /// <summary>
    /// See https://docs.microsoft.com/en-us/ef/core/platforms/#universal-windows-platform for why this is more performant
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Only do it this way when using the main database. When creating a new
        // database, use the default.
        modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotifications);
    }
    static BookDataContext DbContextSingleton = null;
    public static BookDataContext Get([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
    {
        // System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: BookData: Get called from {memberName}");
        if (DbContextSingleton == null)
        {
            DbContextSingleton = new BookDataContext();
        }
        return DbContextSingleton;
    }
    public static void ResetSingleton(string newName)
    {
        if (DbContextSingleton != null)
        {
            DbContextSingleton.SaveChanges();
            DbContextSingleton = null;
        }
        BookDataDatabaseFilename = string.IsNullOrEmpty(newName) ? "BookData.db" : newName;
    }
    public const string BookTypeGutenberg = "gutenberg.org";
    public const string BookTypeUser = "User-imported";

    public void DoMigration()
    {
        //SLOW: the migration is 3 seconds. This is probably the EF+DB startup time.
        this.Database.Migrate();
        logger.LogTrace($"BookData:done migration");
        this.SaveChanges();
        logger.LogTrace($"BookData:done save changes");
    }
}
