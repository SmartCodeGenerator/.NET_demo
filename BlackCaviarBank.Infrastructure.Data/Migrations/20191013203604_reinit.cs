using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlackCaviarBank.Infrastructure.Data.Migrations
{
    public partial class reinit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 13, 23, 35, 51, 517, DateTimeKind.Local).AddTicks(2637),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 10, 13, 23, 25, 36, 378, DateTimeKind.Local).AddTicks(7155));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Notifications",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 13, 23, 35, 51, 515, DateTimeKind.Local).AddTicks(7969),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 10, 13, 23, 25, 36, 377, DateTimeKind.Local).AddTicks(2626));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Cards",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 13, 23, 35, 51, 515, DateTimeKind.Local).AddTicks(1231),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 10, 13, 23, 25, 36, 376, DateTimeKind.Local).AddTicks(5920));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpeningDate",
                table: "Accounts",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 13, 23, 35, 51, 508, DateTimeKind.Local).AddTicks(9910),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 10, 13, 23, 25, 36, 370, DateTimeKind.Local).AddTicks(4143));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 13, 23, 25, 36, 378, DateTimeKind.Local).AddTicks(7155),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 10, 13, 23, 35, 51, 517, DateTimeKind.Local).AddTicks(2637));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 13, 23, 25, 36, 377, DateTimeKind.Local).AddTicks(2626),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 10, 13, 23, 35, 51, 515, DateTimeKind.Local).AddTicks(7969));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Cards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 13, 23, 25, 36, 376, DateTimeKind.Local).AddTicks(5920),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 10, 13, 23, 35, 51, 515, DateTimeKind.Local).AddTicks(1231));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpeningDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 10, 13, 23, 25, 36, 370, DateTimeKind.Local).AddTicks(4143),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 10, 13, 23, 35, 51, 508, DateTimeKind.Local).AddTicks(9910));
        }
    }
}
