using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Voracious.Database.Migrations
{
    /// <inheritdoc />
    public partial class resourceFileAs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileAs",
                table: "Resources",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                comment: "Title to file the resource under");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_FileAs",
                table: "Resources",
                column: "FileAs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Resources_FileAs",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "FileAs",
                table: "Resources");
        }
    }
}
