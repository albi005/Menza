using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Menza.Server.Migrations
{
    /// <inheritdoc />
    public partial class ByteSizedVotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Value",
                table: "Votes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Value",
                table: "Votes",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "INTEGER");
        }
    }
}
