using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WallpaperTimeSheet.Migrations
{
    /// <inheritdoc />
    public partial class fixWorkLogKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkLogs_WorkTasks_WorkTaskId",
                table: "WorkLogs");

            migrationBuilder.AlterColumn<int>(
                name: "WorkTaskId",
                table: "WorkLogs",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLogs_WorkTasks_WorkTaskId",
                table: "WorkLogs",
                column: "WorkTaskId",
                principalTable: "WorkTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkLogs_WorkTasks_WorkTaskId",
                table: "WorkLogs");

            migrationBuilder.AlterColumn<int>(
                name: "WorkTaskId",
                table: "WorkLogs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkLogs_WorkTasks_WorkTaskId",
                table: "WorkLogs",
                column: "WorkTaskId",
                principalTable: "WorkTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
