using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlackCaviarBank.Infrastructure.Data.Migrations
{
    public partial class ContactRelsFix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContactRelationship_ReceiverId",
                table: "ContactRelationship");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 19, 47, 49, 687, DateTimeKind.Local).AddTicks(3899),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 12, 11, 19, 2, 5, 811, DateTimeKind.Local).AddTicks(7808));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Notifications",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 19, 47, 49, 685, DateTimeKind.Local).AddTicks(8096),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 12, 11, 19, 2, 5, 810, DateTimeKind.Local).AddTicks(2186));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Cards",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 19, 47, 49, 685, DateTimeKind.Local).AddTicks(1708),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 12, 11, 19, 2, 5, 809, DateTimeKind.Local).AddTicks(5772));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpeningDate",
                table: "Accounts",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 19, 47, 49, 678, DateTimeKind.Local).AddTicks(6758),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 12, 11, 19, 2, 5, 803, DateTimeKind.Local).AddTicks(1113));

            migrationBuilder.CreateIndex(
                name: "IX_ContactRelationship_ReceiverId_SenderId",
                table: "ContactRelationship",
                columns: new[] { "ReceiverId", "SenderId" },
                unique: true,
                filter: "[ReceiverId] IS NOT NULL AND [SenderId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContactRelationship_ReceiverId_SenderId",
                table: "ContactRelationship");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 19, 2, 5, 811, DateTimeKind.Local).AddTicks(7808),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 11, 19, 47, 49, 687, DateTimeKind.Local).AddTicks(3899));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 19, 2, 5, 810, DateTimeKind.Local).AddTicks(2186),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 11, 19, 47, 49, 685, DateTimeKind.Local).AddTicks(8096));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Cards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 19, 2, 5, 809, DateTimeKind.Local).AddTicks(5772),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 11, 19, 47, 49, 685, DateTimeKind.Local).AddTicks(1708));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpeningDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 19, 2, 5, 803, DateTimeKind.Local).AddTicks(1113),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 11, 19, 47, 49, 678, DateTimeKind.Local).AddTicks(6758));

            migrationBuilder.CreateIndex(
                name: "IX_ContactRelationship_ReceiverId",
                table: "ContactRelationship",
                column: "ReceiverId");
        }
    }
}
