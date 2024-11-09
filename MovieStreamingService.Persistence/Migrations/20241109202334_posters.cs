using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieStreamingService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class posters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BigPoster",
                table: "Movies",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ImageTitle",
                table: "Movies",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BigPoster",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ImageTitle",
                table: "Movies");
        }
    }
}
