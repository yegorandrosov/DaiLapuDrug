using Microsoft.EntityFrameworkCore.Migrations;

namespace DaiLapuDrug.Web.Migrations
{
    public partial class PetFileAttachments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldUrl",
                table: "Pets");

            migrationBuilder.CreateTable(
                name: "PetFileAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetId = table.Column<int>(nullable: false),
                    FileAttachmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetFileAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetFileAttachments_FileAttachments_FileAttachmentId",
                        column: x => x.FileAttachmentId,
                        principalTable: "FileAttachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetFileAttachments_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PetFileAttachments_FileAttachmentId",
                table: "PetFileAttachments",
                column: "FileAttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PetFileAttachments_PetId",
                table: "PetFileAttachments",
                column: "PetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PetFileAttachments");

            migrationBuilder.AddColumn<string>(
                name: "OldUrl",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
