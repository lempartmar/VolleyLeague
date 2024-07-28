using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyLeague.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddADCMainPageSeasonAndLeagueId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AdminDefinedCodes",
                columns: new[] { "Key", "Value", "CreatedDate", "ModifiedDate" },
                values: new object[] { "seasonId-for-main-page", "37", DateTime.UtcNow, DateTime.UtcNow }
            );

            migrationBuilder.InsertData(
                table: "AdminDefinedCodes",
                columns: new[] { "Key", "Value", "CreatedDate", "ModifiedDate" },
                values: new object[] { "leagueId-for-main-page", "1", DateTime.UtcNow, DateTime.UtcNow }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AdminDefinedCodes",
                keyColumn: "Key",
                keyValue: "seasonId-for-main-page"
            );

            migrationBuilder.DeleteData(
                table: "AdminDefinedCodes",
                keyColumn: "Key",
                keyValue: "leagueId-for-main-page"
            );
        }
    }
}
