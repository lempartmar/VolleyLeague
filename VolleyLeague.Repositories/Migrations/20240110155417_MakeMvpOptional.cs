using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyLeague.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class MakeMvpOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "tomasz1_voladmin");


            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Users_MvpId",
                table: "Matches");

            migrationBuilder.AlterColumn<int>(
                name: "MvpId",
                table: "Matches",
                type: "int",
                nullable: true,
                defaultValueSql: "((39))",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "((39))");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Users_MvpId",
                table: "Matches",
                column: "MvpId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Users_MvpId",
                table: "Matches");

            migrationBuilder.AlterColumn<int>(
                name: "MvpId",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValueSql: "((39))",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "((39))");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Users_MvpId",
                table: "Matches",
                column: "MvpId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
