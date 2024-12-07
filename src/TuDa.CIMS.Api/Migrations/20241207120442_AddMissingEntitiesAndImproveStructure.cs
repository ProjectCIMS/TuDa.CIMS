using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuDa.CIMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingEntitiesAndImproveStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hazards_AssetItems_SubstanceId",
                table: "Hazards");

            migrationBuilder.DropIndex(
                name: "IX_Hazards_SubstanceId",
                table: "Hazards");

            migrationBuilder.DropColumn(
                name: "SubstanceId",
                table: "Hazards");

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "AssetItems",
                type: "integer",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ConsumableTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsumableId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AmountChange = table.Column<int>(type: "integer", nullable: false),
                    TransactionReason = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumableTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsumableTransactions_AssetItems_ConsumableId",
                        column: x => x.ConsumableId,
                        principalTable: "AssetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HazardSubstance",
                columns: table => new
                {
                    HazardsId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubstancesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HazardSubstance", x => new { x.HazardsId, x.SubstancesId });
                    table.ForeignKey(
                        name: "FK_HazardSubstance_AssetItems_SubstancesId",
                        column: x => x.SubstancesId,
                        principalTable: "AssetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HazardSubstance_Hazards_HazardsId",
                        column: x => x.HazardsId,
                        principalTable: "Hazards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    WorkingGroupId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkingGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfessorId = table.Column<Guid>(type: "uuid", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingGroups_Person_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WorkingGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    BuyerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Signature = table.Column<byte[]>(type: "bytea", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Completed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_Person_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Purchases_WorkingGroups_WorkingGroupId",
                        column: x => x.WorkingGroupId,
                        principalTable: "WorkingGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    PricePerItem = table.Column<double>(type: "double precision", nullable: false),
                    PurchaseId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseEntries_AssetItems_AssetItemId",
                        column: x => x.AssetItemId,
                        principalTable: "AssetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseEntries_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsumableTransactions_ConsumableId",
                table: "ConsumableTransactions",
                column: "ConsumableId");

            migrationBuilder.CreateIndex(
                name: "IX_HazardSubstance_SubstancesId",
                table: "HazardSubstance",
                column: "SubstancesId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_WorkingGroupId",
                table: "Person",
                column: "WorkingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseEntries_AssetItemId",
                table: "PurchaseEntries",
                column: "AssetItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseEntries_PurchaseId",
                table: "PurchaseEntries",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_BuyerId",
                table: "Purchases",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_WorkingGroupId",
                table: "Purchases",
                column: "WorkingGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingGroups_ProfessorId",
                table: "WorkingGroups",
                column: "ProfessorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Person_WorkingGroups_WorkingGroupId",
                table: "Person",
                column: "WorkingGroupId",
                principalTable: "WorkingGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_WorkingGroups_WorkingGroupId",
                table: "Person");

            migrationBuilder.DropTable(
                name: "ConsumableTransactions");

            migrationBuilder.DropTable(
                name: "HazardSubstance");

            migrationBuilder.DropTable(
                name: "PurchaseEntries");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "WorkingGroups");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.AddColumn<Guid>(
                name: "SubstanceId",
                table: "Hazards",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "AssetItems",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hazards_SubstanceId",
                table: "Hazards",
                column: "SubstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hazards_AssetItems_SubstanceId",
                table: "Hazards",
                column: "SubstanceId",
                principalTable: "AssetItems",
                principalColumn: "Id");
        }
    }
}
