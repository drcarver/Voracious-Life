using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voracious.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookNavigations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookId = table.Column<string>(type: "TEXT", nullable: false),
                    MostRecentNavigationDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    NCatalogViews = table.Column<int>(type: "INTEGER", nullable: false),
                    NSwipeRight = table.Column<int>(type: "INTEGER", nullable: false),
                    NSwipeLeft = table.Column<int>(type: "INTEGER", nullable: false),
                    NReading = table.Column<int>(type: "INTEGER", nullable: false),
                    NSpecificSelection = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrSpot = table.Column<string>(type: "TEXT", nullable: false),
                    CurrStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeMarkedDone = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    FirstNavigationDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    IsDone = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookNavigations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DownloadData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookId = table.Column<string>(type: "TEXT", nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    CurrFileStatus = table.Column<int>(type: "INTEGER", nullable: false),
                    DownloadDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DownloadData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserReviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookId = table.Column<string>(type: "TEXT", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    MostRecentModificationDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    NStars = table.Column<double>(type: "REAL", nullable: false),
                    Review = table.Column<string>(type: "TEXT", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BookId = table.Column<string>(type: "TEXT", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    MostRecentModificationDate = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", nullable: false),
                    Icon = table.Column<string>(type: "TEXT", nullable: false),
                    BackgroundColor = table.Column<string>(type: "TEXT", nullable: false),
                    ForegroundColor = table.Column<string>(type: "TEXT", nullable: false),
                    SelectedText = table.Column<string>(type: "TEXT", nullable: false),
                    BookNoteViewModelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserNotes_BookNotes_BookNoteViewModelId",
                        column: x => x.BookNoteViewModelId,
                        principalTable: "BookNotes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<string>(type: "TEXT", nullable: false),
                    DenormPrimaryAuthor = table.Column<string>(type: "TEXT", nullable: false),
                    BookSource = table.Column<string>(type: "TEXT", nullable: true),
                    BookType = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Imprint = table.Column<string>(type: "TEXT", nullable: true),
                    Issued = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    TitleAlternative = table.Column<string>(type: "TEXT", nullable: true),
                    Language = table.Column<string>(type: "TEXT", nullable: false),
                    LCSH = table.Column<string>(type: "TEXT", nullable: false),
                    LCCN = table.Column<string>(type: "TEXT", nullable: false),
                    PGEditionInfo = table.Column<string>(type: "TEXT", nullable: false),
                    PGProducedBy = table.Column<string>(type: "TEXT", nullable: false),
                    PGNotes = table.Column<string>(type: "TEXT", nullable: false),
                    BookSeries = table.Column<string>(type: "TEXT", nullable: false),
                    LCC = table.Column<string>(type: "TEXT", nullable: false),
                    DenormDownloadDate = table.Column<long>(type: "INTEGER", nullable: false),
                    ReviewId = table.Column<int>(type: "INTEGER", nullable: false),
                    NotesId = table.Column<int>(type: "INTEGER", nullable: false),
                    DownloadDataId = table.Column<int>(type: "INTEGER", nullable: false),
                    NavigationDataId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_BookNavigations_NavigationDataId",
                        column: x => x.NavigationDataId,
                        principalTable: "BookNavigations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_BookNotes_NotesId",
                        column: x => x.NotesId,
                        principalTable: "BookNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_DownloadData_DownloadDataId",
                        column: x => x.DownloadDataId,
                        principalTable: "DownloadData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Books_UserReviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "UserReviews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FilenameAndFormatDataViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    FileType = table.Column<string>(type: "TEXT", nullable: false),
                    LastModified = table.Column<string>(type: "TEXT", nullable: false),
                    BookId = table.Column<string>(type: "TEXT", nullable: false),
                    Extent = table.Column<int>(type: "INTEGER", nullable: false),
                    MimeType = table.Column<string>(type: "TEXT", nullable: false),
                    BookViewModelBookId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilenameAndFormatDataViewModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FilenameAndFormatDataViewModel_Books_BookViewModelBookId",
                        column: x => x.BookViewModelBookId,
                        principalTable: "Books",
                        principalColumn: "BookId");
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Aliases = table.Column<string>(type: "TEXT", nullable: true),
                    BirthDate = table.Column<int>(type: "INTEGER", nullable: false),
                    DeathDate = table.Column<int>(type: "INTEGER", nullable: false),
                    Webpage = table.Column<string>(type: "TEXT", nullable: false),
                    FileAs = table.Column<string>(type: "TEXT", nullable: false),
                    PersonType = table.Column<int>(type: "INTEGER", nullable: false),
                    BookViewModelBookId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_Books_BookViewModelBookId",
                        column: x => x.BookViewModelBookId,
                        principalTable: "Books",
                        principalColumn: "BookId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_DownloadDataId",
                table: "Books",
                column: "DownloadDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_NavigationDataId",
                table: "Books",
                column: "NavigationDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_NotesId",
                table: "Books",
                column: "NotesId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ReviewId",
                table: "Books",
                column: "ReviewId");

            migrationBuilder.CreateIndex(
                name: "IX_FilenameAndFormatDataViewModel_BookViewModelBookId",
                table: "FilenameAndFormatDataViewModel",
                column: "BookViewModelBookId");

            migrationBuilder.CreateIndex(
                name: "IX_People_BookViewModelBookId",
                table: "People",
                column: "BookViewModelBookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotes_BookNoteViewModelId",
                table: "UserNotes",
                column: "BookNoteViewModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilenameAndFormatDataViewModel");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "UserNotes");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "BookNavigations");

            migrationBuilder.DropTable(
                name: "BookNotes");

            migrationBuilder.DropTable(
                name: "DownloadData");

            migrationBuilder.DropTable(
                name: "UserReviews");
        }
    }
}
