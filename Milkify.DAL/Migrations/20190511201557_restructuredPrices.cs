using Microsoft.EntityFrameworkCore.Migrations;

namespace Milkify.DAL.Migrations
{
    public partial class restructuredPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_Route_RouteId",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "ShipmentPrice",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Order");

            migrationBuilder.AlterColumn<long>(
                name: "RouteId",
                table: "Shipment",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_Route_RouteId",
                table: "Shipment",
                column: "RouteId",
                principalTable: "Route",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_Route_RouteId",
                table: "Shipment");

            migrationBuilder.AlterColumn<long>(
                name: "RouteId",
                table: "Shipment",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<decimal>(
                name: "ShipmentPrice",
                table: "Shipment",
                type: "decimal(10, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Order",
                type: "decimal(10, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_Route_RouteId",
                table: "Shipment",
                column: "RouteId",
                principalTable: "Route",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
