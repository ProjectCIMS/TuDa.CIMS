using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuDa.CIMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPredecessorandSucessorToPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SuccessorId",
                table: "Purchases",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_SuccessorId",
                table: "Purchases",
                column: "SuccessorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Purchases_SuccessorId",
                table: "Purchases",
                column: "SuccessorId",
                principalTable: "Purchases",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Purchases_SuccessorId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_SuccessorId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "SuccessorId",
                table: "Purchases");
        }
    }
}
