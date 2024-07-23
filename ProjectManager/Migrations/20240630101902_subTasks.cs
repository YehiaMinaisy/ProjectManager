

using Microsoft.EntityFrameworkCore.Migrations;
namespace ProjectManager.Migrations
{
    /// <inheritdoc />
    public partial class subTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectTaskId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectTaskId",
                table: "Tasks",
                column: "ProjectTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Tasks_ProjectTaskId",
                table: "Tasks",
                column: "ProjectTaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Tasks_ProjectTaskId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_ProjectTaskId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ProjectTaskId",
                table: "Tasks");
        }
    }
}
