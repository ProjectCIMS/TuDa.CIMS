using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuDa.CIMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingAssetItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hazards_AssetItems_ChemicalId",
                table: "Hazards");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "AssetItems");

            migrationBuilder.RenameColumn(
                name: "ChemicalId",
                table: "Hazards",
                newName: "SubstanceId");

            migrationBuilder.RenameIndex(
                name: "IX_Hazards_ChemicalId",
                table: "Hazards",
                newName: "IX_Hazards_SubstanceId");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "AssetItems",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BindingSize",
                table: "AssetItems",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Pressure",
                table: "AssetItems",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "AssetItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "PriceUnit",
                table: "AssetItems",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Purity",
                table: "AssetItems",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Volume",
                table: "AssetItems",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Hazards_AssetItems_SubstanceId",
                table: "Hazards",
                column: "SubstanceId",
                principalTable: "AssetItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hazards_AssetItems_SubstanceId",
                table: "Hazards");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "AssetItems");

            migrationBuilder.DropColumn(
                name: "BindingSize",
                table: "AssetItems");

            migrationBuilder.DropColumn(
                name: "Pressure",
                table: "AssetItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "AssetItems");

            migrationBuilder.DropColumn(
                name: "PriceUnit",
                table: "AssetItems");

            migrationBuilder.DropColumn(
                name: "Purity",
                table: "AssetItems");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "AssetItems");

            migrationBuilder.RenameColumn(
                name: "SubstanceId",
                table: "Hazards",
                newName: "ChemicalId");

            migrationBuilder.RenameIndex(
                name: "IX_Hazards_SubstanceId",
                table: "Hazards",
                newName: "IX_Hazards_ChemicalId");

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "AssetItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Hazards_AssetItems_ChemicalId",
                table: "Hazards",
                column: "ChemicalId",
                principalTable: "AssetItems",
                principalColumn: "Id");
        }
    }
}
