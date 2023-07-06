using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeterParker.Data.Migrations
{
    public partial class VehicleUserAttribtue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_AspNetUsers_UserId",
                table: "Vehicles");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Vehicles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_AspNetUsers_UserId",
                table: "Vehicles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_AspNetUsers_UserId",
                table: "Vehicles");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Vehicles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_AspNetUsers_UserId",
                table: "Vehicles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
