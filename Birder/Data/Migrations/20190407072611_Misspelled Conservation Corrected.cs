using Microsoft.EntityFrameworkCore.Migrations;

namespace Birder.Migrations
{
    public partial class MisspelledConservationCorrected : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bird_ConservationStatus_ConserverationStatusId",
                table: "Bird");

            migrationBuilder.RenameColumn(
                name: "ConservationStatus",
                table: "ConservationStatus",
                newName: "ConservationList");

            migrationBuilder.RenameColumn(
                name: "ConserverationStatusId",
                table: "ConservationStatus",
                newName: "ConservationStatusId");

            migrationBuilder.RenameColumn(
                name: "ConserverationStatusId",
                table: "Bird",
                newName: "ConservationStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Bird_ConserverationStatusId",
                table: "Bird",
                newName: "IX_Bird_ConservationStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bird_ConservationStatus_ConservationStatusId",
                table: "Bird",
                column: "ConservationStatusId",
                principalTable: "ConservationStatus",
                principalColumn: "ConservationStatusId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bird_ConservationStatus_ConservationStatusId",
                table: "Bird");

            migrationBuilder.RenameColumn(
                name: "ConservationList",
                table: "ConservationStatus",
                newName: "ConservationStatus");

            migrationBuilder.RenameColumn(
                name: "ConservationStatusId",
                table: "ConservationStatus",
                newName: "ConserverationStatusId");

            migrationBuilder.RenameColumn(
                name: "ConservationStatusId",
                table: "Bird",
                newName: "ConserverationStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_Bird_ConservationStatusId",
                table: "Bird",
                newName: "IX_Bird_ConserverationStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bird_ConservationStatus_ConserverationStatusId",
                table: "Bird",
                column: "ConserverationStatusId",
                principalTable: "ConservationStatus",
                principalColumn: "ConserverationStatusId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
