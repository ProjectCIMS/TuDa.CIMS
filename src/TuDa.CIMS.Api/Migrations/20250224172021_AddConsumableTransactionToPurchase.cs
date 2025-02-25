using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuDa.CIMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddConsumableTransactionToPurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseId",
                table: "ConsumableTransactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConsumableTransactions_PurchaseId",
                table: "ConsumableTransactions",
                column: "PurchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConsumableTransactions_Purchases_PurchaseId",
                table: "ConsumableTransactions",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConsumableTransactions_Purchases_PurchaseId",
                table: "ConsumableTransactions");

            migrationBuilder.DropIndex(
                name: "IX_ConsumableTransactions_PurchaseId",
                table: "ConsumableTransactions");

            migrationBuilder.DropColumn(
                name: "PurchaseId",
                table: "ConsumableTransactions");
        }
    }
}
