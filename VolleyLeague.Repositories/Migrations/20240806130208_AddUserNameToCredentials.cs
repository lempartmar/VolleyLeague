using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyLeague.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNameToCredentials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<string>(
                name: "UserName",
                schema: "tomasz1_voladmin",
                table: "Credentials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
    name: "LoweredUserName",
    schema: "tomasz1_voladmin",
    table: "Credentials",
    type: "nvarchar(max)",
    nullable: false,
    defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropColumn(
                name: "UserName",
                schema: "tomasz1_voladmin",
                table: "Credentials");

            migrationBuilder.DropColumn(
    name: "LoweredUserName",
    schema: "tomasz1_voladmin",
    table: "Credentials");
        }
    }
}
