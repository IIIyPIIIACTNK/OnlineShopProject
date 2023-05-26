using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShopProject.Migrations
{
    /// <inheritdoc />
    public partial class rework : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SoldItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ItemId = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    ItemCode = table.Column<int>(type: "int", maxLength: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldItems", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "SoldItems");
        }
    }
}
