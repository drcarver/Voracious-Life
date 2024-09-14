using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voracious.Database.Migrations
{
    /// <inheritdoc />
    public partial class person : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    About = table.Column<string>(type: "TEXT", nullable: false, comment: "An unambiguous reference to the resource within a given context. Recommended practice is to identify the resource by means of a string conforming to an identification system"),
                    Name = table.Column<string>(type: "TEXT", nullable: true, comment: "The name of the contributor"),
                    Aliases = table.Column<string>(type: "TEXT", nullable: true, comment: "Alias for the contributor"),
                    BirthDate = table.Column<int>(type: "INTEGER", nullable: true, comment: "The year of birth of the contributor"),
                    DeathDate = table.Column<int>(type: "INTEGER", nullable: true, comment: "The year of death of the contributor"),
                    Webpage = table.Column<string>(type: "TEXT", nullable: true, comment: "The web page for the contributor"),
                    FileAs = table.Column<string>(type: "TEXT", nullable: true, comment: "The name the contributor should be filed nder"),
                    Relator = table.Column<int>(type: "INTEGER", nullable: true, comment: "The nature or genre of the content of the resource. Type includes terms describing general categories, functions, genres, or aggregation levels for content.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.About);
                });

            migrationBuilder.CreateTable(
                name: "PersonModelResourceModel",
                columns: table => new
                {
                    PeopleAbout = table.Column<string>(type: "TEXT", nullable: false),
                    ResourcesAbout = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonModelResourceModel", x => new { x.PeopleAbout, x.ResourcesAbout });
                    table.ForeignKey(
                        name: "FK_PersonModelResourceModel_Person_PeopleAbout",
                        column: x => x.PeopleAbout,
                        principalTable: "Person",
                        principalColumn: "About",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonModelResourceModel_Resources_ResourcesAbout",
                        column: x => x.ResourcesAbout,
                        principalTable: "Resources",
                        principalColumn: "About",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonModelResourceModel_ResourcesAbout",
                table: "PersonModelResourceModel",
                column: "ResourcesAbout");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PersonModelResourceModel");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
