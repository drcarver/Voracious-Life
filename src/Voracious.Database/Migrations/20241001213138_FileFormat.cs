using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voracious.Database.Migrations
{
    /// <inheritdoc />
    public partial class FileFormat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Formats",
                table: "Resources");

            migrationBuilder.CreateTable(
                name: "FileFormats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false, comment: "The key of the file.")
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(type: "TEXT", nullable: false, comment: "The name of the file with this format."),
                    FileType = table.Column<string>(type: "TEXT", nullable: false, comment: "The type of the file with this format."),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "The date and time the file was created."),
                    CurrentFileStatus = table.Column<int>(type: "INTEGER", nullable: false, comment: "Current file status."),
                    DownloadDate = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "The download date"),
                    ResourceAbout = table.Column<string>(type: "TEXT", nullable: false),
                    Extent = table.Column<int>(type: "INTEGER", nullable: false, comment: "The file extent."),
                    MimeType = table.Column<string>(type: "TEXT", nullable: false, comment: "The file Mime Type.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileFormats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileFormats_Resources_ResourceAbout",
                        column: x => x.ResourceAbout,
                        principalTable: "Resources",
                        principalColumn: "About",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FileFormats_FileName",
                table: "FileFormats",
                column: "FileName");

            migrationBuilder.CreateIndex(
                name: "IX_FileFormats_ResourceAbout",
                table: "FileFormats",
                column: "ResourceAbout");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileFormats");

            migrationBuilder.AddColumn<string>(
                name: "Formats",
                table: "Resources",
                type: "TEXT",
                nullable: true);
        }
    }
}
