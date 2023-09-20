using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeterParker.Data.Migrations
{
    public partial class AddHourlyRateToZone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "HourlyRate",
                table: "Zones",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "Passes",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HourlyRate",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Passes");
        }
    }
}
