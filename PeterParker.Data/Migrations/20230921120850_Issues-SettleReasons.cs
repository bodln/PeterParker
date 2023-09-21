using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeterParker.Data.Migrations
{
    public partial class IssuesSettleReasons : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "Tickets",
                newName: "SettleReason");

            migrationBuilder.AddColumn<string>(
                name: "IssueReason",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssueReason",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "SettleReason",
                table: "Tickets",
                newName: "Reason");
        }
    }
}
