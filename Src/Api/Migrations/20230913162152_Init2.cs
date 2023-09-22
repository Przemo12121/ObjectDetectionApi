using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OriginalFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Metadata_Name = table.Column<string>(type: "text", nullable: false),
                    Metadata_Type = table.Column<int>(type: "integer", nullable: false),
                    StorageData_StorageType = table.Column<int>(type: "integer", nullable: false),
                    StorageData_Uri = table.Column<string>(type: "text", nullable: false),
                    OwnerId = table.Column<string>(type: "text", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OriginalFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OriginalFiles_Accounts_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessedFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StorageData_StorageType = table.Column<int>(type: "integer", nullable: false),
                    StorageData_Uri = table.Column<string>(type: "text", nullable: false),
                    ServeData_Url = table.Column<string>(type: "text", nullable: false),
                    Metadata_Name = table.Column<string>(type: "text", nullable: false),
                    Metadata_Type = table.Column<int>(type: "integer", nullable: false),
                    OwnerId = table.Column<string>(type: "text", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessedFiles_Accounts_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ViewersToProcessedFiles",
                columns: table => new
                {
                    ViewerId = table.Column<string>(type: "text", nullable: false),
                    ProcessedFileId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewersToProcessedFiles", x => new { x.ViewerId, x.ProcessedFileId });
                    table.ForeignKey(
                        name: "FK_ViewersToProcessedFiles_Accounts_ViewerId",
                        column: x => x.ViewerId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ViewersToProcessedFiles_ProcessedFiles_ProcessedFileId",
                        column: x => x.ProcessedFileId,
                        principalTable: "ProcessedFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OriginalFiles_OwnerId",
                table: "OriginalFiles",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedFiles_OwnerId",
                table: "ProcessedFiles",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ViewersToProcessedFiles_ProcessedFileId",
                table: "ViewersToProcessedFiles",
                column: "ProcessedFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OriginalFiles");

            migrationBuilder.DropTable(
                name: "ViewersToProcessedFiles");

            migrationBuilder.DropTable(
                name: "ProcessedFiles");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
