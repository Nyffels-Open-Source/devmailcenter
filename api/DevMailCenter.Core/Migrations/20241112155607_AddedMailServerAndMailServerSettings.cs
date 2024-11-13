using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevMailCenter.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedMailServerAndMailServerSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DmcMailServer",
                columns: table => new
                {
                    ServerId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ServerName = table.Column<string>(type: "longtext", nullable: false),
                    ServerType = table.Column<int>(type: "int", nullable: false),
                    ServerActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    ServerCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 11, 12, 15, 56, 7, 117, DateTimeKind.Utc).AddTicks(2911)),
                    ServerModified = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ServerLastUsed = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmcMailServer", x => x.ServerId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DmcMailServerSettings",
                columns: table => new
                {
                    MailServerSettingsId = table.Column<Guid>(type: "char(36)", nullable: false),
                    MailServerSettingsKey = table.Column<string>(type: "longtext", nullable: false),
                    MailServerSettingsValue = table.Column<string>(type: "longtext", nullable: false),
                    MailServerSettingsCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 11, 12, 15, 56, 7, 117, DateTimeKind.Utc).AddTicks(5900)),
                    MailServerSettingsModified = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmcMailServerSettings", x => x.MailServerSettingsId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DmcMailServer_ServerId",
                table: "DmcMailServer",
                column: "ServerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DmcMailServerSettings_MailServerSettingsId",
                table: "DmcMailServerSettings",
                column: "MailServerSettingsId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DmcMailServer");

            migrationBuilder.DropTable(
                name: "DmcMailServerSettings");
        }
    }
}
