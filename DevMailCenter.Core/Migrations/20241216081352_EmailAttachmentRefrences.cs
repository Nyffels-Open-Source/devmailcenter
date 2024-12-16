using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevMailCenter.Core.Migrations
{
    /// <inheritdoc />
    public partial class EmailAttachmentRefrences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 16, 8, 13, 52, 643, DateTimeKind.Utc).AddTicks(4030),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(791));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 16, 8, 13, 52, 643, DateTimeKind.Utc).AddTicks(1710),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 134, DateTimeKind.Utc).AddTicks(7017));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailTransactionCreated",
                table: "DmcEmailTransaction",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 16, 8, 13, 52, 643, DateTimeKind.Utc).AddTicks(6390),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(3803));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailCreated",
                table: "DmcEmail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 16, 8, 13, 52, 643, DateTimeKind.Utc).AddTicks(4830),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(1721));

            migrationBuilder.CreateTable(
                name: "DmcEmailAttachment",
                columns: table => new
                {
                    EmailAttachmentId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EmailAttachmentEmailId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EmailAttachmentMime = table.Column<string>(type: "longtext", nullable: false),
                    EmailAttachmentName = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmcEmailAttachment", x => x.EmailAttachmentId);
                    table.ForeignKey(
                        name: "FK_DmcEmailAttachment_DmcEmail_EmailAttachmentEmailId",
                        column: x => x.EmailAttachmentEmailId,
                        principalTable: "DmcEmail",
                        principalColumn: "EmailId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DmcEmailAttachment_EmailAttachmentEmailId",
                table: "DmcEmailAttachment",
                column: "EmailAttachmentEmailId");

            migrationBuilder.CreateIndex(
                name: "IX_DmcEmailAttachment_EmailAttachmentId",
                table: "DmcEmailAttachment",
                column: "EmailAttachmentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DmcEmailAttachment");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(791),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 16, 8, 13, 52, 643, DateTimeKind.Utc).AddTicks(4030));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 134, DateTimeKind.Utc).AddTicks(7017),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 16, 8, 13, 52, 643, DateTimeKind.Utc).AddTicks(1710));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailTransactionCreated",
                table: "DmcEmailTransaction",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(3803),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 16, 8, 13, 52, 643, DateTimeKind.Utc).AddTicks(6390));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailCreated",
                table: "DmcEmail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(1721),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 16, 8, 13, 52, 643, DateTimeKind.Utc).AddTicks(4830));
        }
    }
}
