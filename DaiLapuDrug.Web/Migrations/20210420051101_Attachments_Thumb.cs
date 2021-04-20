using Microsoft.EntityFrameworkCore.Migrations;

namespace DaiLapuDrug.Web.Migrations
{
    public partial class Attachments_Thumb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageUrl",
                table: "FileAttachments");

            migrationBuilder.AddColumn<int>(
                name: "ImageHeight",
                table: "FileAttachments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ImageWidth",
                table: "FileAttachments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsImage",
                table: "FileAttachments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "FileAttachments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbBlobName",
                table: "FileAttachments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbExtension",
                table: "FileAttachments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThumbImageHeight",
                table: "FileAttachments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ThumbImageWidth",
                table: "FileAttachments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ThumbMimeType",
                table: "FileAttachments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbName",
                table: "FileAttachments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbUrl",
                table: "FileAttachments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "FileAttachments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageHeight",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "ImageWidth",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "IsImage",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "ThumbBlobName",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "ThumbExtension",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "ThumbImageHeight",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "ThumbImageWidth",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "ThumbMimeType",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "ThumbName",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "ThumbUrl",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "FileAttachments");

            migrationBuilder.AddColumn<string>(
                name: "StorageUrl",
                table: "FileAttachments",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
