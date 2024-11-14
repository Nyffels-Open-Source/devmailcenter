using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevMailCenter.Core.Migrations
{
    /// <inheritdoc />
    public partial class LinkedEmailtoaMailServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 8, 42, 29, 447, DateTimeKind.Utc).AddTicks(3557),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 8, 36, 17, 997, DateTimeKind.Utc).AddTicks(7924));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 8, 42, 29, 446, DateTimeKind.Utc).AddTicks(9383),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 8, 36, 17, 997, DateTimeKind.Utc).AddTicks(5185));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailCreated",
                table: "DmcEmail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 8, 42, 29, 447, DateTimeKind.Utc).AddTicks(4614),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 8, 36, 17, 997, DateTimeKind.Utc).AddTicks(8861));

            migrationBuilder.AddColumn<Guid>(
                name: "EmailServerSettingsServerId",
                table: "DmcEmail",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DmcEmail_EmailServerSettingsServerId",
                table: "DmcEmail",
                column: "EmailServerSettingsServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_DmcEmail_DmcMailServer_EmailServerSettingsServerId",
                table: "DmcEmail",
                column: "EmailServerSettingsServerId",
                principalTable: "DmcMailServer",
                principalColumn: "ServerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DmcEmail_DmcMailServer_EmailServerSettingsServerId",
                table: "DmcEmail");

            migrationBuilder.DropIndex(
                name: "IX_DmcEmail_EmailServerSettingsServerId",
                table: "DmcEmail");

            migrationBuilder.DropColumn(
                name: "EmailServerSettingsServerId",
                table: "DmcEmail");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 8, 36, 17, 997, DateTimeKind.Utc).AddTicks(7924),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 8, 42, 29, 447, DateTimeKind.Utc).AddTicks(3557));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 8, 36, 17, 997, DateTimeKind.Utc).AddTicks(5185),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 8, 42, 29, 446, DateTimeKind.Utc).AddTicks(9383));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailCreated",
                table: "DmcEmail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 8, 36, 17, 997, DateTimeKind.Utc).AddTicks(8861),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 8, 42, 29, 447, DateTimeKind.Utc).AddTicks(4614));
        }
    }
}
