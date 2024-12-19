using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuDa.CIMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixHazardsPersonsAndPurchases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_WorkingGroups_WorkingGroupId",
                table: "Person");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_WorkingGroups_WorkingGroupId",
                table: "Purchases");

            migrationBuilder.DropTable(
                name: "HazardSubstance");

            migrationBuilder.DropIndex(
                name: "IX_WorkingGroups_ProfessorId",
                table: "WorkingGroups");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkingGroupId",
                table: "Purchases",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "SubstanceHazard",
                columns: table => new
                {
                    SubstanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    HazardId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubstanceHazard", x => new { x.SubstanceId, x.HazardId });
                    table.ForeignKey(
                        name: "FK_SubstanceHazard_AssetItems_SubstanceId",
                        column: x => x.SubstanceId,
                        principalTable: "AssetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubstanceHazard_Hazards_HazardId",
                        column: x => x.HazardId,
                        principalTable: "Hazards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingGroups_ProfessorId",
                table: "WorkingGroups",
                column: "ProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_SubstanceHazard_HazardId",
                table: "SubstanceHazard",
                column: "HazardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_WorkingGroups_WorkingGroupId",
                table: "Person",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_WorkingGroups_WorkingGroupId",
                table: "Person");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_WorkingGroups_WorkingGroupId",
                table: "Purchases");

            migrationBuilder.DropTable(
                name: "SubstanceHazard");

            migrationBuilder.DropIndex(
                name: "IX_WorkingGroups_ProfessorId",
                table: "WorkingGroups");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkingGroupId",
                table: "Purchases",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_WorkingGroups_ProfessorId",
                table: "WorkingGroups",
                column: "ProfessorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HazardSubstance_SubstancesId",
                table: "HazardSubstance",
                column: "SubstancesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_WorkingGroups_WorkingGroupId",
                table: "Person",
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
    }
}
