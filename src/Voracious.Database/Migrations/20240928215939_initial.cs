using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voracious.Database.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Creators",
                columns: table => new
                {
                    About = table.Column<string>(type: "TEXT", nullable: false, comment: "An unambiguous reference to the resource within a given context. Recommended practice is to identify the resource by means of a string conforming to an identification system"),
                    AlternateScript = table.Column<string>(type: "TEXT", nullable: true),
                    Name = table.Column<string>(type: "TEXT", nullable: true, comment: "The name of the contributor"),
                    Aliases = table.Column<string>(type: "TEXT", nullable: true, comment: "Alias for the contributor"),
                    BirthDate = table.Column<int>(type: "INTEGER", nullable: true, comment: "The year of birth of the contributor"),
                    DeathDate = table.Column<int>(type: "INTEGER", nullable: true, comment: "The year of death of the contributor"),
                    Webpage = table.Column<string>(type: "TEXT", nullable: true, comment: "The web page for the contributor"),
                    FileAs = table.Column<string>(type: "TEXT", nullable: true, comment: "The name the contributor should be filed nder"),
                    Role = table.Column<int>(type: "INTEGER", nullable: true, comment: "The nature or genre of the content of the resource. Type includes terms describing general categories, functions, genres, or aggregation levels for content.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Creators", x => x.About);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    About = table.Column<string>(type: "TEXT", nullable: false, comment: "An unambiguous reference to the resource within a given context. Recommended practice is to identify the resource by means of a string conforming to an identification system"),
                    TitleAlternative = table.Column<string>(type: "TEXT", nullable: true, comment: "An alternative name for the resource."),
                    BookSeries = table.Column<string>(type: "TEXT", nullable: true, comment: "Marc440 - Series statement consisting of a series title alone"),
                    BookType = table.Column<int>(type: "INTEGER", nullable: true, comment: "Most are Text. Human genome project e.g 3501 is Dataset."),
                    CreationProductionCreditsNote = table.Column<string>(type: "TEXT", nullable: true, comment: "Credits for persons or organizations, other than members of the cast, who have participated in the creation and/or production of the work. The introductory term Credits: is usually generated as a display constant."),
                    Description = table.Column<string>(type: "TEXT", nullable: true, comment: "An account of the resource. Description may include but is not limited to: an abstract, a table of contents, a graphical representation, or a free-text account of the resource."),
                    Downloads = table.Column<int>(type: "INTEGER", nullable: true),
                    Formats = table.Column<string>(type: "TEXT", nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: false, comment: "A name given to the resource."),
                    Publisher = table.Column<string>(type: "TEXT", nullable: true, comment: "Book Source - usually Gutenberg"),
                    Issued = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "A related resource of which the described resource is a version, edition, or adaptation. Changes in version imply substantive changes in content rather than differences in format. dcterms:issued Date of formal issuance (e.g., publication) of the resource."),
                    Sources = table.Column<string>(type: "TEXT", nullable: true),
                    Language = table.Column<string>(type: "TEXT", nullable: true, comment: "A language of the resource."),
                    LCC = table.Column<string>(type: "TEXT", nullable: true, comment: "The Library of Congress call number scheme is a standard used in academic libraries nationwide."),
                    LCCN = table.Column<string>(type: "TEXT", nullable: true, comment: "Unique number assigned to a record by the Library of Congress (LC) or a cooperative cataloging partner contributing authority records to the Name Authority Cooperative Program (NACO) database. The field is also assigned to records created by LC for the Library of Congress Subject Headings (LCSH)."),
                    LCSH = table.Column<string>(type: "TEXT", nullable: true, comment: "The set of labeled concepts specified by the Library of Congress Subject Headings."),
                    License = table.Column<string>(type: "TEXT", nullable: true),
                    Rights = table.Column<string>(type: "TEXT", nullable: true),
                    Imprint = table.Column<string>(type: "TEXT", nullable: true, comment: "Information relating to the publication, printing, distribution, issue, release, or production of a work."),
                    PGEditionInfo = table.Column<string>(type: "TEXT", nullable: true, comment: "PG Edition Info."),
                    PGProducedBy = table.Column<string>(type: "TEXT", nullable: true, comment: "PG Edition Info.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.About);
                });

            migrationBuilder.CreateTable(
                name: "CreatorResource",
                columns: table => new
                {
                    CreatorsAbout = table.Column<string>(type: "TEXT", nullable: false),
                    ResourcesAbout = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatorResource", x => new { x.CreatorsAbout, x.ResourcesAbout });
                    table.ForeignKey(
                        name: "FK_CreatorResource_Creators_CreatorsAbout",
                        column: x => x.CreatorsAbout,
                        principalTable: "Creators",
                        principalColumn: "About",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreatorResource_Resources_ResourcesAbout",
                        column: x => x.ResourcesAbout,
                        principalTable: "Resources",
                        principalColumn: "About",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreatorResource_ResourcesAbout",
                table: "CreatorResource",
                column: "ResourcesAbout");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreatorResource");

            migrationBuilder.DropTable(
                name: "Creators");

            migrationBuilder.DropTable(
                name: "Resources");
        }
    }
}
