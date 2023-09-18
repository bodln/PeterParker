using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeterParker.Data.Migrations
{
    public partial class TableOptimisationV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpaces_Garage_GarageId",
                table: "ParkingSpaces");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpaces_Zones_ZoneId",
                table: "ParkingSpaces");

            migrationBuilder.DropTable(
                name: "Garage");

            migrationBuilder.DropIndex(
                name: "IX_ParkingSpaces_GarageId",
                table: "ParkingSpaces");

            migrationBuilder.DropColumn(
                name: "FreeSpaces",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "TotalSpaces",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "GarageId",
                table: "ParkingSpaces");

            migrationBuilder.RenameColumn(
                name: "ZoneId",
                table: "ParkingSpaces",
                newName: "ParkingAreaId");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSpaces_ZoneId",
                table: "ParkingSpaces",
                newName: "IX_ParkingSpaces_ParkingAreaId");

            migrationBuilder.CreateTable(
                name: "ParkingAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkingHours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParkingAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParkingAreas_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingAreas_ZoneId",
                table: "ParkingAreas",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpaces_ParkingAreas_ParkingAreaId",
                table: "ParkingSpaces",
                column: "ParkingAreaId",
                principalTable: "ParkingAreas",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpaces_ParkingAreas_ParkingAreaId",
                table: "ParkingSpaces");

            migrationBuilder.DropTable(
                name: "ParkingAreas");

            migrationBuilder.RenameColumn(
                name: "ParkingAreaId",
                table: "ParkingSpaces",
                newName: "ZoneId");

            migrationBuilder.RenameIndex(
                name: "IX_ParkingSpaces_ParkingAreaId",
                table: "ParkingSpaces",
                newName: "IX_ParkingSpaces_ZoneId");

            migrationBuilder.AddColumn<int>(
                name: "FreeSpaces",
                table: "Zones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalSpaces",
                table: "Zones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GarageId",
                table: "ParkingSpaces",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Garage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZoneId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FreeSpaces = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalSpaces = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkingHours = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Garage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Garage_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpaces_GarageId",
                table: "ParkingSpaces",
                column: "GarageId");

            migrationBuilder.CreateIndex(
                name: "IX_Garage_ZoneId",
                table: "Garage",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpaces_Garage_GarageId",
                table: "ParkingSpaces",
                column: "GarageId",
                principalTable: "Garage",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpaces_Zones_ZoneId",
                table: "ParkingSpaces",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "Id");
        }
    }
}
