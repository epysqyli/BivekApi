using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class FixDataAndCategoriesRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Datasets_DataCategories_Categoryid",
                table: "Datasets");

            migrationBuilder.DropIndex(
                name: "IX_Datasets_Categoryid",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "Categoryid",
                table: "Datasets");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "DataCategories",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "DataCategoryId",
                table: "Datasets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Datasets_DataCategoryId",
                table: "Datasets",
                column: "DataCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Datasets_DataCategories_DataCategoryId",
                table: "Datasets",
                column: "DataCategoryId",
                principalTable: "DataCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Datasets_DataCategories_DataCategoryId",
                table: "Datasets");

            migrationBuilder.DropIndex(
                name: "IX_Datasets_DataCategoryId",
                table: "Datasets");

            migrationBuilder.DropColumn(
                name: "DataCategoryId",
                table: "Datasets");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DataCategories",
                newName: "id");

            migrationBuilder.AddColumn<int>(
                name: "Categoryid",
                table: "Datasets",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Datasets_Categoryid",
                table: "Datasets",
                column: "Categoryid");

            migrationBuilder.AddForeignKey(
                name: "FK_Datasets_DataCategories_Categoryid",
                table: "Datasets",
                column: "Categoryid",
                principalTable: "DataCategories",
                principalColumn: "id");
        }
    }
}
