using Microsoft.EntityFrameworkCore.Migrations;

namespace HomesEngland.Gateway.Migrations
{
    public partial class add_email_to_one_time_token : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "emailaddress",
                table: "authenticationtokens",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "emailaddress",
                table: "authenticationtokens");
        }
    }
}
