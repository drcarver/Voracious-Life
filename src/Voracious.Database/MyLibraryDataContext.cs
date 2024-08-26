using System.Reflection;

using Microsoft.EntityFrameworkCore;

using Voracious.Core.ViewModel;

namespace Voracious.Database;

public class MyLibraryDataContext : DbContext
{
    //private ILogger logger;
    //protected readonly IConfiguration Configuration;

    /// <summary>
    /// Constructor
    /// </summary>
    public MyLibraryDataContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
        string fpath = $@"{folder}\Voracious\";
        Directory.CreateDirectory(fpath);
        Directory.SetCurrentDirectory(fpath);

        optionsBuilder.UseSqlite($@"Filename={fpath}\MyLibrary.db", options =>
        {
            options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
        });
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<BookViewModel> Books { get; set; }
    public DbSet<BookNavigationViewModel> BookNavigations { get; set; }
    public DbSet<BookNoteViewModel> BookNotes { get; set; }
    public DbSet<DownloadDataViewModel> DownloadData { get; set; }
    public DbSet<PersonViewModel> People { get; set; }
    public DbSet<UserNoteViewModel> UserNotes { get; set; }
    public DbSet<UserReviewViewModel> UserReviews { get; set; }
}
