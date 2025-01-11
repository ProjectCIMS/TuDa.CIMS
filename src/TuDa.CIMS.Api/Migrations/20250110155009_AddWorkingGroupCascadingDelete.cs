using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuDa.CIMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkingGroupCascadingDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_WorkingGroups_WorkingGroupId",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_WorkingGroups_WorkingGroupId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_WorkingGroups_ProfessorId",
                table: "WorkingGroups");

            migrationBuilder.AlterColumn<string>(
                name: "Purity",
                table: "AssetItems",
                type: "text",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkingGroups_ProfessorId",
                table: "WorkingGroups",
                column: "ProfessorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_WorkingGroups_WorkingGroupId",
                table: "Persons",
                column: "WorkingGroupId",
                principalTable: "WorkingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_WorkingGroups_WorkingGroupId",
                table: "Purchases",
                column: "WorkingGroupId",
                principalTable: "WorkingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_WorkingGroups_WorkingGroupId",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_WorkingGroups_WorkingGroupId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_WorkingGroups_ProfessorId",
                table: "WorkingGroups");

            migrationBuilder.AlterColumn<double>(
                name: "Purity",
                table: "AssetItems",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkingGroups_ProfessorId",
                table: "WorkingGroups",
                column: "ProfessorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_WorkingGroups_WorkingGroupId",
                table: "Persons",
                column: "WorkingGroupId",
                principalTable: "WorkingGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_WorkingGroups_WorkingGroupId",
                table: "Purchases",
                column: "WorkingGroupId",
                principalTable: "WorkingGroups",
                principalColumn: "Id");
        }
    }
}
