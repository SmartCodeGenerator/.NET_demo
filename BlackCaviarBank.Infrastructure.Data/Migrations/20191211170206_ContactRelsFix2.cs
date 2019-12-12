using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlackCaviarBank.Infrastructure.Data.Migrations
{
    public partial class ContactRelsFix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContactRelationship_AspNetUsers_UserProfileId",
                table: "ContactRelationship");

            migrationBuilder.DropIndex(
                name: "IX_ContactRelationship_UserProfileId",
                table: "ContactRelationship");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "ContactRelationship");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 19, 2, 5, 811, DateTimeKind.Local).AddTicks(7808),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 12, 11, 18, 54, 17, 624, DateTimeKind.Local).AddTicks(1142));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Notifications",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 19, 2, 5, 810, DateTimeKind.Local).AddTicks(2186),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 12, 11, 18, 54, 17, 622, DateTimeKind.Local).AddTicks(5591));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Cards",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 19, 2, 5, 809, DateTimeKind.Local).AddTicks(5772),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 12, 11, 18, 54, 17, 621, DateTimeKind.Local).AddTicks(9458));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpeningDate",
                table: "Accounts",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 19, 2, 5, 803, DateTimeKind.Local).AddTicks(1113),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 12, 11, 18, 54, 17, 615, DateTimeKind.Local).AddTicks(6117));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 18, 54, 17, 624, DateTimeKind.Local).AddTicks(1142),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 11, 19, 2, 5, 811, DateTimeKind.Local).AddTicks(7808));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 18, 54, 17, 622, DateTimeKind.Local).AddTicks(5591),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 11, 19, 2, 5, 810, DateTimeKind.Local).AddTicks(2186));

            migrationBuilder.AddColumn<string>(
                name: "UserProfileId",
                table: "ContactRelationship",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Cards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 18, 54, 17, 621, DateTimeKind.Local).AddTicks(9458),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 11, 19, 2, 5, 809, DateTimeKind.Local).AddTicks(5772));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpeningDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 18, 54, 17, 615, DateTimeKind.Local).AddTicks(6117),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 11, 19, 2, 5, 803, DateTimeKind.Local).AddTicks(1113));

            migrationBuilder.CreateIndex(
                name: "IX_ContactRelationship_UserProfileId",
                table: "ContactRelationship",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactRelationship_AspNetUsers_UserProfileId",
                table: "ContactRelationship",
                column: "UserProfileId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
