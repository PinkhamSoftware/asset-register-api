using Microsoft.EntityFrameworkCore.Migrations;

namespace HomesEngland.Gateway.Migrations
{
    public partial class additional_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "householdincome",
                table: "assets", 
                nullable:true);

            migrationBuilder.AddColumn<decimal>(
                name: "estimatedvaluation",
                table: "assets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "householdincome",
                table: "assets");

            migrationBuilder.DropColumn(
                name: "estimatedvaluation",
                table: "assets");
        }
    }
}
