using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevMailCenter.Core.Migrations
{
    /// <inheritdoc />
    public partial class CreationofEmailTransactionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 15, 8, 55, 805, DateTimeKind.Utc).AddTicks(5805),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 11, 40, 42, 109, DateTimeKind.Utc).AddTicks(756));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 15, 8, 55, 805, DateTimeKind.Utc).AddTicks(1389),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 11, 40, 42, 108, DateTimeKind.Utc).AddTicks(6399));

            migrationBuilder.AddColumn<string>(
                name: "ReceiverName",
                table: "DmcEmailReceiver",
                type: "longtext",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailCreated",
                table: "DmcEmail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 15, 8, 55, 805, DateTimeKind.Utc).AddTicks(7025),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 11, 40, 42, 109, DateTimeKind.Utc).AddTicks(1944));

            migrationBuilder.CreateTable(
                name: "DmcEmailTransaction",
                columns: table => new
                {
                    EmailTransactionId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EmailTransactionEmailId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EmailTransactionRawResponse = table.Column<string>(type: "longtext", nullable: false),
                    EmailTransactionCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 11, 14, 15, 8, 55, 805, DateTimeKind.Utc).AddTicks(9546))
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmcEmailTransaction", x => x.EmailTransactionId);
                    table.ForeignKey(
                        name: "FK_DmcEmailTransaction_DmcEmail_EmailTransactionEmailId",
                        column: x => x.EmailTransactionEmailId,
                        principalTable: "DmcEmail",
                        principalColumn: "EmailId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DmcEmailTransaction_EmailTransactionEmailId",
                table: "DmcEmailTransaction",
                column: "EmailTransactionEmailId");

            migrationBuilder.CreateIndex(
                name: "IX_DmcEmailTransaction_EmailTransactionId",
                table: "DmcEmailTransaction",
                column: "EmailTransactionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DmcEmailTransaction");

            migrationBuilder.DropColumn(
                name: "ReceiverName",
                table: "DmcEmailReceiver");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 11, 40, 42, 109, DateTimeKind.Utc).AddTicks(756),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 15, 8, 55, 805, DateTimeKind.Utc).AddTicks(5805));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 11, 40, 42, 108, DateTimeKind.Utc).AddTicks(6399),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 15, 8, 55, 805, DateTimeKind.Utc).AddTicks(1389));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailCreated",
                table: "DmcEmail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 11, 40, 42, 109, DateTimeKind.Utc).AddTicks(1944),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 15, 8, 55, 805, DateTimeKind.Utc).AddTicks(7025));
        }
    }
}
