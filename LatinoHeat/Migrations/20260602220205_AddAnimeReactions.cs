using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LatinoHeat.Migrations
{
    /// <inheritdoc />
    public partial class AddAnimeReactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimeReactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnimeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsLiked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimeReactions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimeReactions_AnimeId_UserId",
                table: "AnimeReactions",
                columns: new[] { "AnimeId", "UserId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimeReactions");
        }
    }
}
