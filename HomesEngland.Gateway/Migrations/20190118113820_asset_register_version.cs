using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace HomesEngland.Gateway.Migrations
{
    public partial class asset_register_version : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "assetregisterversionid",
                table: "assets",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssetEntity_assetregisterversionid",
                table: "assets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "assetregisterversions",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    modifieddatetime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assetregisterversions", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assets_AssetEntity_assetregisterversionid",
                table: "assets",
                column: "AssetEntity_assetregisterversionid");

            migrationBuilder.AddForeignKey(
                name: "FK_assets_assetregisterversions_AssetEntity_assetregisterversi~",
                table: "assets",
                column: "AssetEntity_assetregisterversionid",
                principalTable: "assetregisterversions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assets_assetregisterversions_AssetEntity_assetregisterversi~",
                table: "assets");

            migrationBuilder.DropTable(
                name: "assetregisterversions");

            migrationBuilder.DropIndex(
                name: "IX_assets_AssetEntity_assetregisterversionid",
                table: "assets");

            migrationBuilder.DropColumn(
                name: "assetregisterversionid",
                table: "assets");

            migrationBuilder.DropColumn(
                name: "AssetEntity_assetregisterversionid",
                table: "assets");
        }
    }
}
