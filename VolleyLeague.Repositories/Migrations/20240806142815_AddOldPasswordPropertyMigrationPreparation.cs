using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyLeague.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddOldPasswordPropertyMigrationPreparation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OldPassword",
                schema: "tomasz1_voladmin",
                table: "Credentials",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldPassword",
                schema: "tomasz1_voladmin",
                table: "Credentials");
        }
    }
}
