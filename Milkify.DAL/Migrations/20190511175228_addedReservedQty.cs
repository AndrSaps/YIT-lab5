using Microsoft.EntityFrameworkCore.Migrations;

namespace Milkify.DAL.Migrations
{
    public partial class addedReservedQty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReservedSupplyQuantity",
                table: "Inventory",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservedSupplyQuantity",
                table: "Inventory");
        }
    }
}
