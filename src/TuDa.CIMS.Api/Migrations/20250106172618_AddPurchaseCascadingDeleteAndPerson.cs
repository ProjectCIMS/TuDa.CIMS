using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuDa.CIMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseCascadingDeleteAndPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_WorkingGroups_WorkingGroupId",
                table: "Person");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseEntries_Purchases_PurchaseId",
                table: "PurchaseEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Person_BuyerId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingGroups_Person_ProfessorId",
                table: "WorkingGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Person",
                table: "Person");

            migrationBuilder.RenameTable(
                name: "Person",
                newName: "Persons");

            migrationBuilder.RenameIndex(
                name: "IX_Person_WorkingGroupId",
                table: "Persons",
                newName: "IX_Persons_WorkingGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Persons",
                table: "Persons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_WorkingGroups_WorkingGroupId",
                table: "Persons",
                column: "WorkingGroupId",
                principalTable: "WorkingGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseEntries_Purchases_PurchaseId",
                table: "PurchaseEntries",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Persons_BuyerId",
                table: "Purchases",
                column: "BuyerId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingGroups_Persons_ProfessorId",
                table: "WorkingGroups",
                column: "ProfessorId",
                principalTable: "Persons",
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
                name: "FK_PurchaseEntries_Purchases_PurchaseId",
                table: "PurchaseEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Persons_BuyerId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingGroups_Persons_ProfessorId",
                table: "WorkingGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Persons",
                table: "Persons");

            migrationBuilder.RenameTable(
                name: "Persons",
                newName: "Person");

            migrationBuilder.RenameIndex(
                name: "IX_Persons_WorkingGroupId",
                table: "Person",
                newName: "IX_Person_WorkingGroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Person",
                table: "Person",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_WorkingGroups_WorkingGroupId",
                table: "Person",
                column: "WorkingGroupId",
                principalTable: "WorkingGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseEntries_Purchases_PurchaseId",
                table: "PurchaseEntries",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Person_BuyerId",
                table: "Purchases",
                column: "BuyerId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingGroups_Person_ProfessorId",
                table: "WorkingGroups",
                column: "ProfessorId",
                principalTable: "Person",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
