using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevMailCenter.Core.Migrations
{
    /// <inheritdoc />
    public partial class EmailPriorityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 11, 40, 42, 109, DateTimeKind.Utc).AddTicks(756),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 10, 17, 14, 938, DateTimeKind.Utc).AddTicks(735));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 11, 40, 42, 108, DateTimeKind.Utc).AddTicks(6399),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 10, 17, 14, 937, DateTimeKind.Utc).AddTicks(6094));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailCreated",
                table: "DmcEmail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 11, 40, 42, 109, DateTimeKind.Utc).AddTicks(1944),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 10, 17, 14, 938, DateTimeKind.Utc).AddTicks(1869));

            migrationBuilder.AddColumn<int>(
                name: "EmailPriority",
                table: "DmcEmail",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailPriority",
                table: "DmcEmail");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 10, 17, 14, 938, DateTimeKind.Utc).AddTicks(735),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 11, 40, 42, 109, DateTimeKind.Utc).AddTicks(756));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 10, 17, 14, 937, DateTimeKind.Utc).AddTicks(6094),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 11, 40, 42, 108, DateTimeKind.Utc).AddTicks(6399));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailCreated",
                table: "DmcEmail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 10, 17, 14, 938, DateTimeKind.Utc).AddTicks(1869),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 11, 40, 42, 109, DateTimeKind.Utc).AddTicks(1944));
        }
    }
}
