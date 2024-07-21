using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolleyLeague.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "tomasz1_voladmin");

            migrationBuilder.AlterColumn<int>(
                name: "MvpId",
                table: "Matches",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "((39))");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MvpId",
                table: "Matches",
                type: "int",
                nullable: true,
                defaultValueSql: "((39))",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
