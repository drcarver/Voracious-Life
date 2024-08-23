using System.Reflection;

using Microsoft.EntityFrameworkCore;

using Voracious.Core.ViewModel;

namespace Voracious.Database;

public class VoraciousDataContext : DbContext
{
    //private ILogger logger;
    //protected readonly IConfiguration Configuration;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="loggerFactory">The logger factory</param>
    /// <param name="configuration">The configuration</param>
    //public VoraciousDataContext(
    //    ILoggerFactory loggerFactory,
    //    IConfiguration configuration)
    public VoraciousDataContext()
    {
        //Configuration = configuration;
        //logger = loggerFactory.CreateLogger<VoraciousDataContext>();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
        string fpath = $@"{folder}\Voracious\";
        Directory.CreateDirectory(fpath);
        Directory.SetCurrentDirectory(fpath);

        optionsBuilder.UseSqlite($@"Filename={fpath}\ebook.db", options =>
        {
            options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
        });
        base.OnConfiguring(optionsBuilder);

        //logger.LogInformation($"Creating database at {fpath}");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Map table names
        //modelBuilder.Entity<Blog>().ToTable("Blogs", "test");
        //modelBuilder.Entity<Blog>(entity =>
        //{
        //    entity.HasKey(e => e.BlogId);
        //    entity.HasIndex(e => e.Title).IsUnique();
        //    entity.Property(e => e.DateTimeAdd).HasDefaultValueSql("CURRENT_TIMESTAMP");
        //});
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
