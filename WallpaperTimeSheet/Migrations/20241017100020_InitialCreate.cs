using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WallpaperTimeSheet.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Label = table.Column<string>(type: "TEXT", nullable: false),
                    Color = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkLogs",
                columns: table => new
                {
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WorkTaskId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkLogs", x => x.DateTime);
                    table.ForeignKey(
                        name: "FK_WorkLogs_WorkTasks_WorkTaskId",
                        column: x => x.WorkTaskId,
                        principalTable: "WorkTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkLogs_WorkTaskId",
                table: "WorkLogs",
                column: "WorkTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkLogs");

            migrationBuilder.DropTable(
                name: "WorkTasks");
        }
    }
}
