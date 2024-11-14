using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevMailCenter.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedEmailAndEmailReceiver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 8, 36, 17, 997, DateTimeKind.Utc).AddTicks(7924),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 13, 15, 17, 17, 812, DateTimeKind.Utc).AddTicks(8819));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 14, 8, 36, 17, 997, DateTimeKind.Utc).AddTicks(5185),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 13, 15, 17, 17, 812, DateTimeKind.Utc).AddTicks(5604));

            migrationBuilder.CreateTable(
                name: "DmcEmail",
                columns: table => new
                {
                    EmailId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EmailSubject = table.Column<string>(type: "longtext", nullable: false),
                    EmailMessage = table.Column<string>(type: "longtext", nullable: false),
                    EmailCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2024, 11, 14, 8, 36, 17, 997, DateTimeKind.Utc).AddTicks(8861)),
                    EmailSendRequested = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EmailCompleted = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EmailStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EmailRawServerResponse = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmcEmail", x => x.EmailId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DmcEmailReceiver",
                columns: table => new
                {
                    EmailReceiverId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EmailReceiverEmailId = table.Column<Guid>(type: "char(36)", nullable: false),
                    EmailReceiverReceiverEmail = table.Column<string>(type: "longtext", nullable: false),
                    EmailReceiverType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DmcEmailReceiver", x => x.EmailReceiverId);
                    table.ForeignKey(
                        name: "FK_DmcEmailReceiver_DmcEmail_EmailReceiverEmailId",
                        column: x => x.EmailReceiverEmailId,
                        principalTable: "DmcEmail",
                        principalColumn: "EmailId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DmcEmail_EmailId",
                table: "DmcEmail",
                column: "EmailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DmcEmailReceiver_EmailReceiverEmailId",
                table: "DmcEmailReceiver",
                column: "EmailReceiverEmailId");

            migrationBuilder.CreateIndex(
                name: "IX_DmcEmailReceiver_EmailReceiverId",
                table: "DmcEmailReceiver",
                column: "EmailReceiverId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DmcEmailReceiver");

            migrationBuilder.DropTable(
                name: "DmcEmail");

            migrationBuilder.AlterColumn<DateTime>(
                name: "MailServerSettingsCreated",
                table: "DmcMailServerSettings",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 13, 15, 17, 17, 812, DateTimeKind.Utc).AddTicks(8819),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 8, 36, 17, 997, DateTimeKind.Utc).AddTicks(7924));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerCreated",
                table: "DmcMailServer",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2024, 11, 13, 15, 17, 17, 812, DateTimeKind.Utc).AddTicks(5604),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2024, 11, 14, 8, 36, 17, 997, DateTimeKind.Utc).AddTicks(5185));
        }
    }
}
