using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevMailCenter.Core.Migrations
{
    /// <inheritdoc />
    public partial class MailServerSettingsSecret : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(791),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 15, 8, 55, 805, DateTimeKind.Utc).AddTicks(5805));

            migrationBuilder.AddColumn<bool>(
                name: "MailServerSettingsSecret",
                table: "DmcMailServerSettings",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 134, DateTimeKind.Utc).AddTicks(7017),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 15, 8, 55, 805, DateTimeKind.Utc).AddTicks(1389));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailTransactionCreated",
                table: "DmcEmailTransaction",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(3803),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 15, 8, 55, 805, DateTimeKind.Utc).AddTicks(9546));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailCreated",
                table: "DmcEmail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(1721),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 15, 8, 55, 805, DateTimeKind.Utc).AddTicks(7025));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MailServerSettingsSecret",
                table: "DmcMailServerSettings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 15, 8, 55, 805, DateTimeKind.Utc).AddTicks(5805),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(791));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 15, 8, 55, 805, DateTimeKind.Utc).AddTicks(1389),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 134, DateTimeKind.Utc).AddTicks(7017));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailTransactionCreated",
                table: "DmcEmailTransaction",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 15, 8, 55, 805, DateTimeKind.Utc).AddTicks(9546),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(3803));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailCreated",
                table: "DmcEmail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 15, 8, 55, 805, DateTimeKind.Utc).AddTicks(7025),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(1721));
        }
    }
}
