using Microsoft.EntityFrameworkCore.Migrations;

namespace HomesEngland.Gateway.Migrations
{
    public partial class change_table_name : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_assets",
                table: "assets");

            migrationBuilder.RenameTable(
                name: "assets",
                newName: "asset");

            migrationBuilder.AddPrimaryKey(
                name: "PK_asset",
                table: "asset",
                column: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_asset",
                table: "asset");

            migrationBuilder.RenameTable(
                name: "asset",
                newName: "assets");

            migrationBuilder.AddPrimaryKey(
                name: "PK_assets",
                table: "assets",
                column: "id");
        }
    }
}
