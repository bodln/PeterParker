using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeterParker.Data.Migrations
{
    public partial class AddReasonToTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Tickets");
        }
    }
}
