using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevMailCenter.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRawServerResponsefromEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailRawServerResponse",
                table: "DmcEmail");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 10, 17, 14, 938, DateTimeKind.Utc).AddTicks(735),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 8, 42, 29, 447, DateTimeKind.Utc).AddTicks(3557));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 10, 17, 14, 937, DateTimeKind.Utc).AddTicks(6094),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 8, 42, 29, 446, DateTimeKind.Utc).AddTicks(9383));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailCreated",
                table: "DmcEmail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 10, 17, 14, 938, DateTimeKind.Utc).AddTicks(1869),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 8, 42, 29, 447, DateTimeKind.Utc).AddTicks(4614));

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailModified",
                table: "DmcEmail",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailModified",
                table: "DmcEmail");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 8, 42, 29, 447, DateTimeKind.Utc).AddTicks(3557),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 10, 17, 14, 938, DateTimeKind.Utc).AddTicks(735));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 8, 42, 29, 446, DateTimeKind.Utc).AddTicks(9383),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 10, 17, 14, 937, DateTimeKind.Utc).AddTicks(6094));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailCreated",
                table: "DmcEmail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 8, 42, 29, 447, DateTimeKind.Utc).AddTicks(4614),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 10, 17, 14, 938, DateTimeKind.Utc).AddTicks(1869));

            migrationBuilder.AddColumn<string>(
                name: "EmailRawServerResponse",
                table: "DmcEmail",
                type: "longtext",
                nullable: true);
        }
    }
}
