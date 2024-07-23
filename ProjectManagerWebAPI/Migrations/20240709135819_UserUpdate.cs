using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjectManagerWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a6d6479-7f0b-4d9b-89b2-9a5f588a2d7d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb989132-00c4-4f78-8dc4-d39a77af84ea");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "261ac95e-0d0a-4600-ae49-25515ad535cf", null, "Admin", "ADMIN" },
                    { "79c50772-5a63-4a85-8e4e-0d520a111812", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "261ac95e-0d0a-4600-ae49-25515ad535cf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "79c50772-5a63-4a85-8e4e-0d520a111812");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9a6d6479-7f0b-4d9b-89b2-9a5f588a2d7d", null, "User", "USER" },
                    { "bb989132-00c4-4f78-8dc4-d39a77af84ea", null, "Admin", "ADMIN" }
                });
        }
    }
}
