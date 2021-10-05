using Microsoft.EntityFrameworkCore.Migrations;

namespace Milkify.DAL.Migrations
{
    public partial class removedDriverFromShipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factory_Location_LocationId",
                table: "Factory");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_AspNetUsers_DriverId",
                table: "Shipment");

            migrationBuilder.DropIndex(
                name: "IX_Shipment_DriverId",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "DriverId",
                table: "Shipment");

            migrationBuilder.AlterColumn<long>(
                name: "LocationId",
                table: "Factory",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Factory_Location_LocationId",
                table: "Factory",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factory_Location_LocationId",
                table: "Factory");

            migrationBuilder.AddColumn<long>(
                name: "DriverId",
                table: "Shipment",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "LocationId",
                table: "Factory",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_DriverId",
                table: "Shipment",
                column: "DriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Factory_Location_LocationId",
                table: "Factory",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_AspNetUsers_DriverId",
                table: "Shipment",
                column: "DriverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
