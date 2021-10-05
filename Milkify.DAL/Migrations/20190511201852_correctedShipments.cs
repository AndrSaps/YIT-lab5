using Microsoft.EntityFrameworkCore.Migrations;

namespace Milkify.DAL.Migrations
{
    public partial class correctedShipments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_Route_RouteId",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Shipment");

            migrationBuilder.AlterColumn<long>(
                name: "RouteId",
                table: "Shipment",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_Route_RouteId",
                table: "Shipment",
                column: "RouteId",
                principalTable: "Route",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_Route_RouteId",
                table: "Shipment");

            migrationBuilder.AlterColumn<long>(
                name: "RouteId",
                table: "Shipment",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StatusId",
                table: "Shipment",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_Route_RouteId",
                table: "Shipment",
                column: "RouteId",
                principalTable: "Route",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
