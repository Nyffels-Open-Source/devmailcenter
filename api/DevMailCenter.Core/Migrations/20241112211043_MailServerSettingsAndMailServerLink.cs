using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace devmailcenter.db.Migrations
{
    /// <inheritdoc />
    public partial class MailServerSettingsAndMailServerLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 12, 21, 10, 43, 481, DateTimeKind.Utc).AddTicks(716),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 12, 15, 56, 7, 117, DateTimeKind.Utc).AddTicks(5900));

            migrationBuilder.AddColumn<Guid>(
                name: "MailServerId",
                table: "DmcMailServerSettings",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MailServerSettingsServerId",
                table: "DmcMailServerSettings",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 12, 21, 10, 43, 480, DateTimeKind.Utc).AddTicks(4894),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 12, 15, 56, 7, 117, DateTimeKind.Utc).AddTicks(2911));

            migrationBuilder.CreateIndex(
                name: "ix_DmcMailServer",
                table: "DmcMailServerSettings",
                column: "MailServerSettingsServerId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_DmcMailServerSettings_DmcMailServer_MailServerSettingsServer~",
                table: "DmcMailServerSettings",
                column: "MailServerSettingsServerId",
                principalTable: "DmcMailServer",
                principalColumn: "ServerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DmcMailServerSettings_DmcMailServer_MailServerId",
                table: "DmcMailServerSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_DmcMailServerSettings_DmcMailServer_MailServerSettingsServer~",
                table: "DmcMailServerSettings");

            migrationBuilder.DropIndex(
                name: "ix_DmcMailServer",
                table: "DmcMailServerSettings");

            migrationBuilder.DropIndex(
                name: "IX_DmcMailServerSettings_MailServerId",
                table: "DmcMailServerSettings");

            migrationBuilder.DropColumn(
                name: "MailServerId",
                table: "DmcMailServerSettings");

            migrationBuilder.DropColumn(
                name: "MailServerSettingsServerId",
                table: "DmcMailServerSettings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 12, 15, 56, 7, 117, DateTimeKind.Utc).AddTicks(5900),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 12, 21, 10, 43, 481, DateTimeKind.Utc).AddTicks(716));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 12, 15, 56, 7, 117, DateTimeKind.Utc).AddTicks(2911),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 12, 21, 10, 43, 480, DateTimeKind.Utc).AddTicks(4894));
        }
    }
}
