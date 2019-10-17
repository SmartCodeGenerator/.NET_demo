using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlackCaviarBank.Infrastructure.Data.Migrations
{
    public partial class CardAndAccountNumbersToStringType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 14, 20, 35, 11, 510, DateTimeKind.Local).AddTicks(7789),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 10, 14, 17, 59, 42, 405, DateTimeKind.Local).AddTicks(528));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Notifications",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 14, 20, 35, 11, 509, DateTimeKind.Local).AddTicks(1934),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 10, 14, 17, 59, 42, 403, DateTimeKind.Local).AddTicks(4181));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Cards",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 14, 20, 35, 11, 508, DateTimeKind.Local).AddTicks(3123),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 10, 14, 17, 59, 42, 402, DateTimeKind.Local).AddTicks(7487));

            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                table: "Cards",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpeningDate",
                table: "Accounts",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 14, 20, 35, 11, 502, DateTimeKind.Local).AddTicks(759),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 10, 14, 17, 59, 42, 396, DateTimeKind.Local).AddTicks(3858));

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "Accounts",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 14, 17, 59, 42, 405, DateTimeKind.Local).AddTicks(528),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 10, 14, 20, 35, 11, 510, DateTimeKind.Local).AddTicks(7789));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 14, 17, 59, 42, 403, DateTimeKind.Local).AddTicks(4181),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 10, 14, 20, 35, 11, 509, DateTimeKind.Local).AddTicks(1934));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Cards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 14, 17, 59, 42, 402, DateTimeKind.Local).AddTicks(7487),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 10, 14, 20, 35, 11, 508, DateTimeKind.Local).AddTicks(3123));

            migrationBuilder.AlterColumn<int>(
                name: "CardNumber",
                table: "Cards",
                type: "int",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 16);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpeningDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 14, 17, 59, 42, 396, DateTimeKind.Local).AddTicks(3858),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 10, 14, 20, 35, 11, 502, DateTimeKind.Local).AddTicks(759));

            migrationBuilder.AlterColumn<int>(
                name: "AccountNumber",
                table: "Accounts",
                type: "int",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);
        }
    }
}
