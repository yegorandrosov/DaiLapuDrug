using Microsoft.EntityFrameworkCore.Migrations;

namespace DaiLapuDrug.Web.Migrations
{
    public partial class Pet_IsSterilized : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSterilized",
                table: "Pets",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSterilized",
                table: "Pets");
        }
    }
}
