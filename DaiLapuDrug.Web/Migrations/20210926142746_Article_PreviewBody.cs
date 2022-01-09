using Microsoft.EntityFrameworkCore.Migrations;

namespace DaiLapuDrug.Web.Migrations
{
    public partial class Article_PreviewBody : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PreviewBody",
                table: "Articles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreviewBody",
                table: "Articles");
        }
    }
}
