using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademiaApp.Migrations
{
    /// <inheritdoc />
    public partial class Inicial5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treinos_AspNetUsers_AlunoId",
                table: "Treinos");

            migrationBuilder.DropForeignKey(
                name: "FK_Treinos_AspNetUsers_PersonalId",
                table: "Treinos");

            migrationBuilder.AddForeignKey(
                name: "FK_Treinos_AspNetUsers_AlunoId",
                table: "Treinos",
                column: "AlunoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Treinos_AspNetUsers_PersonalId",
                table: "Treinos",
                column: "PersonalId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treinos_AspNetUsers_AlunoId",
                table: "Treinos");

            migrationBuilder.DropForeignKey(
                name: "FK_Treinos_AspNetUsers_PersonalId",
                table: "Treinos");

            migrationBuilder.AddForeignKey(
                name: "FK_Treinos_AspNetUsers_AlunoId",
                table: "Treinos",
                column: "AlunoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Treinos_AspNetUsers_PersonalId",
                table: "Treinos",
                column: "PersonalId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
