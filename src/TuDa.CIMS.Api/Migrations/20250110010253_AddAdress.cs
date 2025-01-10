using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuDa.CIMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAdress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "Person",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Person",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Person",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Person",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Person",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Purity",
                table: "AssetItems",
                type: "text",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    ZipCode = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Person_AddressId",
                table: "Person",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Person_Addresses_AddressId",
                table: "Person",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Person_Addresses_AddressId",
                table: "Person");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Person_AddressId",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Person");

            migrationBuilder.AlterColumn<double>(
                name: "Purity",
                table: "AssetItems",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
