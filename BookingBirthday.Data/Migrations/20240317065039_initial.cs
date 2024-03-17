using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingBirthday.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "category_id",
                table: "Package",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.category_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Package_category_id",
                table: "Package",
                column: "category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Package_Categories_category_id",
                table: "Package",
                column: "category_id",
                principalTable: "Categories",
                principalColumn: "category_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Package_Categories_category_id",
                table: "Package");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Package_category_id",
                table: "Package");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "Package");
        }
    }
}
