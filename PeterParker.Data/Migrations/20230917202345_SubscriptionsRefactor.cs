using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeterParker.Data.Migrations
{
    public partial class SubscriptionsRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Vehicles_VehicleId",
                table: "Subscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Zones_ZoneId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Zones_Subscriptions_SubscriptionId",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Zones_SubscriptionId",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ZoneId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_VehicleId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "ParkingSpaceId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Subscriptions");

            migrationBuilder.AddColumn<Guid>(
                name: "ParkingSpaceGuid",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ZoneGuid",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "Subscriptions",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParkingSpaceGuid",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ZoneGuid",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Subscriptions");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Zones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionId",
                table: "Zones",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParkingSpaceId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ZoneId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Zones_SubscriptionId",
                table: "Zones",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ZoneId",
                table: "Tickets",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_VehicleId",
                table: "Subscriptions",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Vehicles_VehicleId",
                table: "Subscriptions",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Zones_ZoneId",
                table: "Tickets",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_Subscriptions_SubscriptionId",
                table: "Zones",
                column: "SubscriptionId",
                principalTable: "Subscriptions",
                principalColumn: "Id");
        }
    }
}
