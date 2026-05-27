using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LatinoHeat.Migrations
{
    /// <inheritdoc />
    public partial class CategoryToGenre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Animes",
                newName: "Genre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Animes",
                newName: "Category");
        }
    }
}
