using Microsoft.EntityFrameworkCore.Migrations;

namespace HomesEngland.Gateway.Migrations
{
    public partial class asset_register_version_relations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assets_assetregisterversions_AssetEntity_assetregisterversi~",
                table: "assets");

            migrationBuilder.RenameColumn(
                name: "AssetEntity_assetregisterversionid",
                table: "assets",
                newName: "AssetRegisterVersionEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_assets_AssetEntity_assetregisterversionid",
                table: "assets",
                newName: "IX_assets_AssetRegisterVersionEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_assets_assetregisterversionid",
                table: "assets",
                column: "assetregisterversionid");

            migrationBuilder.AddForeignKey(
                name: "FK_assets_assetregisterversions_AssetRegisterVersionEntityId",
                table: "assets",
                column: "AssetRegisterVersionEntityId",
                principalTable: "assetregisterversions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_assets_assetregisterversions_assetregisterversionid",
                table: "assets",
                column: "assetregisterversionid",
                principalTable: "assetregisterversions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_assets_assetregisterversions_AssetRegisterVersionEntityId",
                table: "assets");

            migrationBuilder.DropForeignKey(
                name: "FK_assets_assetregisterversions_assetregisterversionid",
                table: "assets");

            migrationBuilder.DropIndex(
                name: "IX_assets_assetregisterversionid",
                table: "assets");

            migrationBuilder.RenameColumn(
                name: "AssetRegisterVersionEntityId",
                table: "assets",
                newName: "AssetEntity_assetregisterversionid");

            migrationBuilder.RenameIndex(
                name: "IX_assets_AssetRegisterVersionEntityId",
                table: "assets",
                newName: "IX_assets_AssetEntity_assetregisterversionid");

            migrationBuilder.AddForeignKey(
                name: "FK_assets_assetregisterversions_AssetEntity_assetregisterversi~",
                table: "assets",
                column: "AssetEntity_assetregisterversionid",
                principalTable: "assetregisterversions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
