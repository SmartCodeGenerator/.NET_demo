using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlackCaviarBank.Infrastructure.Data.Migrations
{
    public partial class DbFix3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 14, 17, 59, 42, 405, DateTimeKind.Local).AddTicks(528),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 10, 13, 23, 35, 51, 517, DateTimeKind.Local).AddTicks(2637));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Notifications",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 14, 17, 59, 42, 403, DateTimeKind.Local).AddTicks(4181),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 10, 13, 23, 35, 51, 515, DateTimeKind.Local).AddTicks(7969));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Cards",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 14, 17, 59, 42, 402, DateTimeKind.Local).AddTicks(7487),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 10, 13, 23, 35, 51, 515, DateTimeKind.Local).AddTicks(1231));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpeningDate",
                table: "Accounts",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 14, 17, 59, 42, 396, DateTimeKind.Local).AddTicks(3858),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 10, 13, 23, 35, 51, 508, DateTimeKind.Local).AddTicks(9910));

            migrationBuilder.CreateIndex(
                name: "IX_Services_Name",
                table: "Services",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardNumber",
                table: "Cards",
                column: "CardNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountNumber",
                table: "Accounts",
                column: "AccountNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Services_Name",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CardNumber",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_AccountNumber",
                table: "Accounts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 13, 23, 35, 51, 517, DateTimeKind.Local).AddTicks(2637),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 10, 14, 17, 59, 42, 405, DateTimeKind.Local).AddTicks(528));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 13, 23, 35, 51, 515, DateTimeKind.Local).AddTicks(7969),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 10, 14, 17, 59, 42, 403, DateTimeKind.Local).AddTicks(4181));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Cards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 13, 23, 35, 51, 515, DateTimeKind.Local).AddTicks(1231),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 10, 14, 17, 59, 42, 402, DateTimeKind.Local).AddTicks(7487));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpeningDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 13, 23, 35, 51, 508, DateTimeKind.Local).AddTicks(9910),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 10, 14, 17, 59, 42, 396, DateTimeKind.Local).AddTicks(3858));
        }
    }
}
