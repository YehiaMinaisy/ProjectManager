
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManager.Migrations
{
    /// <inheritdoc />
    public partial class addAttachmentToTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Attachment_AttachmentId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "Attachment");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_AttachmentId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "Tasks");

            migrationBuilder.AddColumn<byte[]>(
                name: "AttachmentData",
                table: "Tasks",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentName",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentType",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttachmentData",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AttachmentName",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AttachmentType",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "AttachmentId",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AttachmentId",
                table: "Tasks",
                column: "AttachmentId",
                unique: true,
                filter: "[AttachmentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Attachment_AttachmentId",
                table: "Tasks",
                column: "AttachmentId",
                principalTable: "Attachment",
                principalColumn: "Id");
        }
    }
}
