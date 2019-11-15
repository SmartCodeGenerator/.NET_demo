using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlackCaviarBank.Infrastructure.Data.Migrations
{
    public partial class ProfileImageAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                nullable: false,
                defaultValue: new DateTime(2019, 11, 15, 2, 8, 25, 403, DateTimeKind.Local).AddTicks(5612),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 11, 14, 20, 23, 27, 759, DateTimeKind.Local).AddTicks(5979));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Notifications",
                nullable: false,
                defaultValue: new DateTime(2019, 11, 15, 2, 8, 25, 402, DateTimeKind.Local).AddTicks(45),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 11, 14, 20, 23, 27, 758, DateTimeKind.Local).AddTicks(35));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Cards",
                nullable: false,
                defaultValue: new DateTime(2019, 11, 15, 2, 8, 25, 401, DateTimeKind.Local).AddTicks(3587),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 11, 14, 20, 23, 27, 757, DateTimeKind.Local).AddTicks(3390));

            migrationBuilder.AlterColumn<bool>(
                name: "IsBanned",
                table: "AspNetUsers",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImage",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpeningDate",
                table: "Accounts",
                nullable: false,
                defaultValue: new DateTime(2019, 11, 15, 2, 8, 25, 393, DateTimeKind.Local).AddTicks(8717),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 11, 14, 20, 23, 27, 750, DateTimeKind.Local).AddTicks(4146));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 11, 14, 20, 23, 27, 759, DateTimeKind.Local).AddTicks(5979),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 11, 15, 2, 8, 25, 403, DateTimeKind.Local).AddTicks(5612));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 11, 14, 20, 23, 27, 758, DateTimeKind.Local).AddTicks(35),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 11, 15, 2, 8, 25, 402, DateTimeKind.Local).AddTicks(45));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Cards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 11, 14, 20, 23, 27, 757, DateTimeKind.Local).AddTicks(3390),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 11, 15, 2, 8, 25, 401, DateTimeKind.Local).AddTicks(3587));

            migrationBuilder.AlterColumn<bool>(
                name: "IsBanned",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpeningDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 11, 14, 20, 23, 27, 750, DateTimeKind.Local).AddTicks(4146),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 11, 15, 2, 8, 25, 393, DateTimeKind.Local).AddTicks(8717));
        }
    }
}
