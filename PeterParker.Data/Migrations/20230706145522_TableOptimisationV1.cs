using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeterParker.Data.Migrations
{
    public partial class TableOptimisationV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpaces_Zones_ZoneId",
                table: "ParkingSpaces");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "ZoneId",
                table: "ParkingSpaces",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleId",
                table: "ParkingSpaces",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "GarageId",
                table: "ParkingSpaces",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ParkingSpaceId",
                table: "Tickets",
                column: "ParkingSpaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ZoneId",
                table: "Tickets",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_VehicleId",
                table: "Subscriptions",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpaces_GarageId",
                table: "ParkingSpaces",
                column: "GarageId");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpaces_VehicleId",
                table: "ParkingSpaces",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpaces_Garage_GarageId",
                table: "ParkingSpaces",
                column: "GarageId",
                principalTable: "Garage",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpaces_Vehicles_VehicleId",
                table: "ParkingSpaces",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpaces_Zones_ZoneId",
                table: "ParkingSpaces",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Vehicles_VehicleId",
                table: "Subscriptions",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_ParkingSpaces_ParkingSpaceId",
                table: "Tickets",
                column: "ParkingSpaceId",
                principalTable: "ParkingSpaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Zones_ZoneId",
                table: "Tickets",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpaces_Garage_GarageId",
                table: "ParkingSpaces");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpaces_Vehicles_VehicleId",
                table: "ParkingSpaces");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpaces_Zones_ZoneId",
                table: "ParkingSpaces");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Vehicles_VehicleId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_ParkingSpaces_ParkingSpaceId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Zones_ZoneId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ParkingSpaceId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ZoneId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_VehicleId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_ParkingSpaces_GarageId",
                table: "ParkingSpaces");

            migrationBuilder.DropIndex(
                name: "IX_ParkingSpaces_VehicleId",
                table: "ParkingSpaces");

            migrationBuilder.DropColumn(
                name: "GarageId",
                table: "ParkingSpaces");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ZoneId",
                table: "ParkingSpaces",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VehicleId",
                table: "ParkingSpaces",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpaces_Zones_ZoneId",
                table: "ParkingSpaces",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
