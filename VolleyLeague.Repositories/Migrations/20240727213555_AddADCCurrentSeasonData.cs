using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyLeague.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddADCCurrentSeasonData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AdminDefinedCodes",
                columns: new[] { "Key", "Value", "CreatedDate", "ModifiedDate" },
                values: new object[] { "current-season-for-main-page", "SEZON 2023/2024", DateTime.UtcNow, DateTime.UtcNow }
);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdminDefinedCodes",
                keyColumn: "Key",
                keyValue: "current-season-for-main-page");
        }
    }
}
