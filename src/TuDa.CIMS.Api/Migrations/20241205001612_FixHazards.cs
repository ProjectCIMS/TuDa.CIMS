using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuDa.CIMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixHazards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hazards_AssetItems_ChemicalId",
                table: "Hazards");

            migrationBuilder.DropIndex(
                name: "IX_Hazards_ChemicalId",
                table: "Hazards");

            migrationBuilder.DropColumn(
                name: "ChemicalId",
                table: "Hazards");

            migrationBuilder.CreateTable(
                name: "ChemicalHazard",
                columns: table => new
                {
                    ChemicalsId = table.Column<Guid>(type: "uuid", nullable: false),
                    HazardsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChemicalHazard", x => new { x.ChemicalsId, x.HazardsId });
                    table.ForeignKey(
                        name: "FK_ChemicalHazard_AssetItems_ChemicalsId",
                        column: x => x.ChemicalsId,
                        principalTable: "AssetItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChemicalHazard_Hazards_HazardsId",
                        column: x => x.HazardsId,
                        principalTable: "Hazards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChemicalHazard_HazardsId",
                table: "ChemicalHazard",
                column: "HazardsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChemicalHazard");

            migrationBuilder.AddColumn<Guid>(
                name: "ChemicalId",
                table: "Hazards",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hazards_ChemicalId",
                table: "Hazards",
                column: "ChemicalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Hazards_AssetItems_ChemicalId",
                table: "Hazards",
                column: "ChemicalId",
                principalTable: "AssetItems",
                principalColumn: "Id");
        }
    }
}
