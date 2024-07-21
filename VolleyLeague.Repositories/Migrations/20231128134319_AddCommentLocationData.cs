using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VolleyLeague.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentLocationData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "tomasz1_voladmin");


            migrationBuilder.InsertData(
                table: "CommentLocations",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Article" },
                    { 2, "Team" },
                    { 3, "Player" },
                    { 4, "Match" },
                    { 5, "Thread" },
                    { 6, "PrivateMessage" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CommentLocations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CommentLocations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CommentLocations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CommentLocations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CommentLocations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CommentLocations",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
