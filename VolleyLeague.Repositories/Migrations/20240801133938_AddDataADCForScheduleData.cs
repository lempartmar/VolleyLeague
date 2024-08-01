using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyLeague.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddDataADCForScheduleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AdminDefinedCodes",
                columns: new[] { "Key", "Value", "CreatedDate", "ModifiedDate" },
                values: new object[] { "leagueId-for-schedule", "1", DateTime.UtcNow, DateTime.UtcNow }
            );
            migrationBuilder.InsertData(
                table: "AdminDefinedCodes",
                columns: new[] { "Key", "Value", "CreatedDate", "ModifiedDate" },
                values: new object[] { "seasonId-for-schedule", "37", DateTime.UtcNow, DateTime.UtcNow }
            );
            migrationBuilder.InsertData(
                table: "AdminDefinedCodes",
                columns: new[] { "Key", "Value", "CreatedDate", "ModifiedDate" },
                values: new object[] { "roundId-for-schedule", "1237", DateTime.UtcNow, DateTime.UtcNow }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
