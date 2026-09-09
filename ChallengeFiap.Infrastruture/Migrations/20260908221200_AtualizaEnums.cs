using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChallengeFiap.Infrastruture.Migrations
{
    /// <inheritdoc />
    public partial class AtualizaEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "st_consulta",
                table: "T_CLYVO_CONSULTA",
                type: "NVARCHAR2(2000)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.AlterColumn<string>(
                name: "st_vacinacao",
                table: "T_CLYVO_CARTEIRAVACINAL",
                type: "NVARCHAR2(2000)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "st_consulta",
                table: "T_CLYVO_CONSULTA",
                type: "NUMBER(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)");

            migrationBuilder.AlterColumn<int>(
                name: "st_vacinacao",
                table: "T_CLYVO_CARTEIRAVACINAL",
                type: "NUMBER(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)");
        }
    }
}
