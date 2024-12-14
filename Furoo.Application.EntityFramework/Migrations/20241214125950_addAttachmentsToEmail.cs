using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevMailCenter.Core.Migrations
{
    /// <inheritdoc />
    public partial class addAttachmentsToEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 14, 12, 59, 49, 919, DateTimeKind.Utc).AddTicks(6552),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(791));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 14, 12, 59, 49, 919, DateTimeKind.Utc).AddTicks(2741),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 134, DateTimeKind.Utc).AddTicks(7017));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailTransactionCreated",
                table: "DmcEmailTransaction",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 14, 12, 59, 49, 920, DateTimeKind.Utc).AddTicks(610),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(3803));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailCreated",
                table: "DmcEmail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 12, 14, 12, 59, 49, 919, DateTimeKind.Utc).AddTicks(7691),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(1721));

            migrationBuilder.CreateTable(
                name: "EmailAttachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    EmailId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Mime = table.Column<string>(type: "longtext", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailAttachment_DmcEmail_EmailId",
                        column: x => x.EmailId,
                        principalTable: "DmcEmail",
                        principalColumn: "EmailId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAttachment_EmailId",
                table: "EmailAttachment",
                column: "EmailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailAttachment");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(791),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 14, 12, 59, 49, 919, DateTimeKind.Utc).AddTicks(6552));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 134, DateTimeKind.Utc).AddTicks(7017),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 14, 12, 59, 49, 919, DateTimeKind.Utc).AddTicks(2741));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailTransactionCreated",
                table: "DmcEmailTransaction",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(3803),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 14, 12, 59, 49, 920, DateTimeKind.Utc).AddTicks(610));

            migrationBuilder.AlterColumn<DateTime>(
                name: "EmailCreated",
                table: "DmcEmail",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 26, 13, 40, 59, 135, DateTimeKind.Utc).AddTicks(1721),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 12, 14, 12, 59, 49, 919, DateTimeKind.Utc).AddTicks(7691));
        }
    }
}
