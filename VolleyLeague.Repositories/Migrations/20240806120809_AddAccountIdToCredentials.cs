using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyLeague.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountIdToCredentials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                schema: "tomasz1_voladmin",
                table: "Credentials",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "tomasz1_voladmin",
                table: "Credentials");
        }
    }
}
