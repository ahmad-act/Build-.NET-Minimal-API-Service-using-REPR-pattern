using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookInformationService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbSetBookInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false),
                    Available = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbSetBookInformation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbSetBookInformation_Title",
                table: "DbSetBookInformation",
                column: "Title",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbSetBookInformation");
        }
    }
}
