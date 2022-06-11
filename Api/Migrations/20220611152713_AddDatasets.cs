using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class AddDatasets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dataset_DataCategories_Categoryid",
                table: "Dataset");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dataset",
                table: "Dataset");

            migrationBuilder.RenameTable(
                name: "Dataset",
                newName: "Datasets");

            migrationBuilder.RenameIndex(
                name: "IX_Dataset_Categoryid",
                table: "Datasets",
                newName: "IX_Datasets_Categoryid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Datasets",
                table: "Datasets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Datasets_DataCategories_Categoryid",
                table: "Datasets",
                column: "Categoryid",
                principalTable: "DataCategories",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Datasets_DataCategories_Categoryid",
                table: "Datasets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Datasets",
                table: "Datasets");

            migrationBuilder.RenameTable(
                name: "Datasets",
                newName: "Dataset");

            migrationBuilder.RenameIndex(
                name: "IX_Datasets_Categoryid",
                table: "Dataset",
                newName: "IX_Dataset_Categoryid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dataset",
                table: "Dataset",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dataset_DataCategories_Categoryid",
                table: "Dataset",
                column: "Categoryid",
                principalTable: "DataCategories",
                principalColumn: "id");
        }
    }
}
