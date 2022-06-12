using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class FixDataCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Datasets_DataCategories_Categoryid",
                table: "Datasets");

            migrationBuilder.RenameColumn(
                name: "Categoryid",
                table: "Datasets",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Datasets_Categoryid",
                table: "Datasets",
                newName: "IX_Datasets_CategoryId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "DataCategories",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Datasets_DataCategories_CategoryId",
                table: "Datasets",
                column: "CategoryId",
                principalTable: "DataCategories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Datasets_DataCategories_CategoryId",
                table: "Datasets");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Datasets",
                newName: "Categoryid");

            migrationBuilder.RenameIndex(
                name: "IX_Datasets_CategoryId",
                table: "Datasets",
                newName: "IX_Datasets_Categoryid");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DataCategories",
                newName: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Datasets_DataCategories_Categoryid",
                table: "Datasets",
                column: "Categoryid",
                principalTable: "DataCategories",
                principalColumn: "id");
        }
    }
}
