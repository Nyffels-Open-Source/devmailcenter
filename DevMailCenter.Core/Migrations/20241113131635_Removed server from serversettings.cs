using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevMailCenter.Core.Migrations
{
    /// <inheritdoc />
    public partial class Removedserverfromserversettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DmcMailServerSettings_DmcMailServer_MailServerId",
                table: "DmcMailServerSettings");

            migrationBuilder.DropIndex(
                name: "IX_DmcMailServerSettings_MailServerId",
                table: "DmcMailServerSettings");

            migrationBuilder.DropColumn(
                name: "MailServerId",
                table: "DmcMailServerSettings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 13, 13, 16, 35, 205, DateTimeKind.Utc).AddTicks(4945),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 13, 13, 5, 5, 214, DateTimeKind.Utc).AddTicks(3219));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 13, 13, 16, 35, 205, DateTimeKind.Utc).AddTicks(1342),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 13, 13, 5, 5, 213, DateTimeKind.Utc).AddTicks(4606));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 13, 13, 5, 5, 214, DateTimeKind.Utc).AddTicks(3219),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 13, 13, 16, 35, 205, DateTimeKind.Utc).AddTicks(4945));

            migrationBuilder.AddColumn<Guid>(
                name: "MailServerId",
                table: "DmcMailServerSettings",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 13, 13, 5, 5, 213, DateTimeKind.Utc).AddTicks(4606),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 13, 13, 16, 35, 205, DateTimeKind.Utc).AddTicks(1342));

            migrationBuilder.CreateIndex(
                name: "IX_DmcMailServerSettings_MailServerId",
                table: "DmcMailServerSettings",
                column: "MailServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_DmcMailServerSettings_DmcMailServer_MailServerId",
                table: "DmcMailServerSettings",
                column: "MailServerId",
                principalTable: "DmcMailServer",
                principalColumn: "ServerId");
        }
    }
}
