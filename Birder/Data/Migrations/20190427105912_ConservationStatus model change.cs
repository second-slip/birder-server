using Microsoft.EntityFrameworkCore.Migrations;

namespace Birder.Migrations
{
    public partial class ConservationStatusmodelchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConservationListColourCode",
                table: "ConservationStatus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConservationListColourCode",
                table: "ConservationStatus");
        }
    }
}
