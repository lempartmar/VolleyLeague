using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyLeague.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class AddCorrectionToCreadentialsTableForDataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Credentials_UserId",
                schema: "tomasz1_voladmin",
                table: "Credentials");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                schema: "tomasz1_voladmin",
                table: "Credentials",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                schema: "tomasz1_voladmin",
                table: "Credentials",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "tomasz1_voladmin",
                table: "Credentials",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_UserId",
                schema: "tomasz1_voladmin",
                table: "Credentials",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Credentials_UserId",
                schema: "tomasz1_voladmin",
                table: "Credentials");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                schema: "tomasz1_voladmin",
                table: "Credentials",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                schema: "tomasz1_voladmin",
                table: "Credentials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "tomasz1_voladmin",
                table: "Credentials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_UserId",
                schema: "tomasz1_voladmin",
                table: "Credentials",
                column: "UserId",
                unique: true);
        }
    }
}
