using Microsoft.EntityFrameworkCore.Migrations;

namespace Banking.API.Migrations
{
    public partial class Nicknames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccNickname",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccNickname",
                table: "Accounts");
        }
    }
}
