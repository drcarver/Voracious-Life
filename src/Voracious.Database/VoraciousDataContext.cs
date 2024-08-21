using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Voracious.Database.Model;

namespace Voracious.Database;

public class VoraciousDataContext : DbContext
{
    private ILogger logger;

    public VoraciousDataContext(ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<VoraciousDataContext>();
    }

    public DbSet<BookModel> Books { get; set; }
    public DbSet<BookNavigationModel> BookNavigations { get; set; }
    public DbSet<BookNoteModel> BookNotes { get; set; }
    public DbSet<DownloadDataModel> DownloadData { get; set; }
    public DbSet<PersonModel> People { get; set; }
    public DbSet<UserNoteModel> UserNotes { get; set; }
    public DbSet<UserReviewModel> UserReviews { get; set; }
}
