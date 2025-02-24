using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuDa.CIMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRoomInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetItems_Rooms_RoomId",
                table: "AssetItems");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_AssetItems_RoomId",
                table: "AssetItems");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "AssetItems");

            migrationBuilder.AddColumn<int>(
                name: "Room",
                table: "AssetItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Room",
                table: "AssetItems");

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "AssetItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssetItems_RoomId",
                table: "AssetItems",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetItems_Rooms_RoomId",
                table: "AssetItems",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
