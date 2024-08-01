using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyLeague.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddADCYoutubeLinkData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AdminDefinedCodes",
                columns: new[] { "Key", "Value", "CreatedDate", "ModifiedDate" },
                values: new object[] { "youtube-link1", "https://www.youtube.com/embed/DID8UK3gLj8", DateTime.UtcNow, DateTime.UtcNow });
            migrationBuilder.InsertData(
                table: "AdminDefinedCodes",
                columns: new[] { "Key", "Value", "CreatedDate", "ModifiedDate" },
                values: new object[] { "youtube-link2", "https://www.youtube.com/embed/ZgunK7Wgfvc", DateTime.UtcNow, DateTime.UtcNow });
            migrationBuilder.InsertData(
                table: "AdminDefinedCodes",
                columns: new[] { "Key", "Value", "CreatedDate", "ModifiedDate" },
                values: new object[] { "youtube-link3", "https://www.youtube.com/embed/imtHlApQ-EU", DateTime.UtcNow, DateTime.UtcNow });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
