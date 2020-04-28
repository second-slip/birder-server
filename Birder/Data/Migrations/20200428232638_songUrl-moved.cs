using Microsoft.EntityFrameworkCore.Migrations;

namespace Birder.Migrations
{
    public partial class songUrlmoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SongUrl",
                table: "Bird");

            migrationBuilder.AddColumn<string>(
                name: "SongUrl",
                table: "TweetDay",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SongUrl",
                table: "TweetDay");

            migrationBuilder.AddColumn<string>(
                name: "SongUrl",
                table: "Bird",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
