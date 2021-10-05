using Microsoft.EntityFrameworkCore.Migrations;

namespace Milkify.DAL.Migrations
{
    public partial class correctedOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ShipmentId",
                table: "Order",
                nullable: true,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ShipmentId",
                table: "Order",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);
        }
    }
}
