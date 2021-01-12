using Microsoft.EntityFrameworkCore.Migrations;

namespace RecipesApp.Domain.Infrastructure.Migrations
{
    public partial class AddCreatedByUpdatedByToBaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Recipes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Recipes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CreatedBy",
                table: "Recipes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_UpdatedBy",
                table: "Recipes",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AspNetUsers_CreatedBy",
                table: "Recipes",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AspNetUsers_UpdatedBy",
                table: "Recipes",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_AspNetUsers_CreatedBy",
                table: "Recipes");

            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_AspNetUsers_UpdatedBy",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_CreatedBy",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_UpdatedBy",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Recipes");
        }
    }
}
