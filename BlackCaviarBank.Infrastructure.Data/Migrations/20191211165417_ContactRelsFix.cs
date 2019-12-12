using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlackCaviarBank.Infrastructure.Data.Migrations
{
    public partial class ContactRelsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 18, 54, 17, 624, DateTimeKind.Local).AddTicks(1142),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 11, 15, 2, 8, 25, 403, DateTimeKind.Local).AddTicks(5612));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Notifications",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 18, 54, 17, 622, DateTimeKind.Local).AddTicks(5591),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 11, 15, 2, 8, 25, 402, DateTimeKind.Local).AddTicks(45));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Cards",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 18, 54, 17, 621, DateTimeKind.Local).AddTicks(9458),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 11, 15, 2, 8, 25, 401, DateTimeKind.Local).AddTicks(3587));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpeningDate",
                table: "Accounts",
                nullable: false,
                defaultValue: new DateTime(2019, 12, 11, 18, 54, 17, 615, DateTimeKind.Local).AddTicks(6117),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2019, 11, 15, 2, 8, 25, 393, DateTimeKind.Local).AddTicks(8717));

            migrationBuilder.CreateTable(
                name: "ContactRelationship",
                columns: table => new
                {
                    ContactRelationshipId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsApproved = table.Column<bool>(nullable: false, defaultValue: false),
                    SenderId = table.Column<string>(nullable: true),
                    ReceiverId = table.Column<string>(nullable: true),
                    UserProfileId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactRelationship", x => x.ContactRelationshipId);
                    table.ForeignKey(
                        name: "FK_ContactRelationship_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactRelationship_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactRelationship_AspNetUsers_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactRelationship_ReceiverId",
                table: "ContactRelationship",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactRelationship_SenderId",
                table: "ContactRelationship",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactRelationship_UserProfileId",
                table: "ContactRelationship",
                column: "UserProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactRelationship");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 11, 15, 2, 8, 25, 403, DateTimeKind.Local).AddTicks(5612),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 11, 18, 54, 17, 624, DateTimeKind.Local).AddTicks(1142));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "Notifications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 11, 15, 2, 8, 25, 402, DateTimeKind.Local).AddTicks(45),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 11, 18, 54, 17, 622, DateTimeKind.Local).AddTicks(5591));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpirationDate",
                table: "Cards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 11, 15, 2, 8, 25, 401, DateTimeKind.Local).AddTicks(3587),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 11, 18, 54, 17, 621, DateTimeKind.Local).AddTicks(9458));

            migrationBuilder.AlterColumn<DateTime>(
                name: "OpeningDate",
                table: "Accounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2019, 11, 15, 2, 8, 25, 393, DateTimeKind.Local).AddTicks(8717),
                oldClrType: typeof(DateTime),
                oldDefaultValue: new DateTime(2019, 12, 11, 18, 54, 17, 615, DateTimeKind.Local).AddTicks(6117));
        }
    }
}
