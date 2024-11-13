using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevMailCenter.Core.Migrations
{
    /// <inheritdoc />
    public partial class MadeDatesNullableForMailServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 12, 22, 57, 51, 56, DateTimeKind.Utc).AddTicks(8849),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 12, 21, 10, 43, 481, DateTimeKind.Utc).AddTicks(716));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerModified",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerLastUsed",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 12, 22, 57, 51, 56, DateTimeKind.Utc).AddTicks(3215),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 12, 21, 10, 43, 480, DateTimeKind.Utc).AddTicks(4894));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 12, 21, 10, 43, 481, DateTimeKind.Utc).AddTicks(716),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 12, 22, 57, 51, 56, DateTimeKind.Utc).AddTicks(8849));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerModified",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerLastUsed",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 12, 21, 10, 43, 480, DateTimeKind.Utc).AddTicks(4894),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 12, 22, 57, 51, 56, DateTimeKind.Utc).AddTicks(3215));
        }
    }
}
