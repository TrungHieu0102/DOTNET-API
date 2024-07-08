using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DOTNET_API.Migrations
{
    /// <inheritdoc />
    public partial class seedingrole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0ee0f91b-f57d-4404-946c-0620652f73f8", null, "User", "user" },
                    { "e91e030d-bad5-423e-bf8d-cae3b6e26398", null, "Admin", "admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0ee0f91b-f57d-4404-946c-0620652f73f8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e91e030d-bad5-423e-bf8d-cae3b6e26398");
        }
    }
}
