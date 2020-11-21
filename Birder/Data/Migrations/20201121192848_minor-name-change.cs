using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Birder.Migrations
{
    public partial class minornamechange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Position_Observation_ObservationId",
                table: "Position");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Position",
                table: "Position");

            migrationBuilder.RenameTable(
                name: "Position",
                newName: "ObservationPosition");

            migrationBuilder.RenameIndex(
                name: "IX_Position_ObservationId",
                table: "ObservationPosition",
                newName: "IX_ObservationPosition_ObservationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ObservationPosition",
                table: "ObservationPosition",
                column: "ObservationPositionId");

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 1,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2020, 11, 21, 19, 28, 48, 63, DateTimeKind.Local).AddTicks(1086), new DateTime(2020, 11, 21, 19, 28, 48, 66, DateTimeKind.Local).AddTicks(4947) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 2,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2020, 11, 21, 19, 28, 48, 67, DateTimeKind.Local).AddTicks(9109), new DateTime(2020, 11, 21, 19, 28, 48, 67, DateTimeKind.Local).AddTicks(9135) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 3,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2020, 11, 21, 19, 28, 48, 67, DateTimeKind.Local).AddTicks(9268), new DateTime(2020, 11, 21, 19, 28, 48, 67, DateTimeKind.Local).AddTicks(9273) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 4,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2020, 11, 21, 19, 28, 48, 67, DateTimeKind.Local).AddTicks(9294), new DateTime(2020, 11, 21, 19, 28, 48, 67, DateTimeKind.Local).AddTicks(9298) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 5,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2020, 11, 21, 19, 28, 48, 67, DateTimeKind.Local).AddTicks(9402), new DateTime(2020, 11, 21, 19, 28, 48, 67, DateTimeKind.Local).AddTicks(9407) });

            migrationBuilder.AddForeignKey(
                name: "FK_ObservationPosition_Observation_ObservationId",
                table: "ObservationPosition",
                column: "ObservationId",
                principalTable: "Observation",
                principalColumn: "ObservationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ObservationPosition_Observation_ObservationId",
                table: "ObservationPosition");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ObservationPosition",
                table: "ObservationPosition");

            migrationBuilder.RenameTable(
                name: "ObservationPosition",
                newName: "Position");

            migrationBuilder.RenameIndex(
                name: "IX_ObservationPosition_ObservationId",
                table: "Position",
                newName: "IX_Position_ObservationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Position",
                table: "Position",
                column: "ObservationPositionId");

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 1,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2020, 11, 19, 14, 26, 41, 792, DateTimeKind.Local).AddTicks(5738), new DateTime(2020, 11, 19, 14, 26, 41, 798, DateTimeKind.Local).AddTicks(1804) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 2,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2020, 11, 19, 14, 26, 41, 801, DateTimeKind.Local).AddTicks(969), new DateTime(2020, 11, 19, 14, 26, 41, 801, DateTimeKind.Local).AddTicks(1064) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 3,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2020, 11, 19, 14, 26, 41, 801, DateTimeKind.Local).AddTicks(1401), new DateTime(2020, 11, 19, 14, 26, 41, 801, DateTimeKind.Local).AddTicks(1420) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 4,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2020, 11, 19, 14, 26, 41, 801, DateTimeKind.Local).AddTicks(1472), new DateTime(2020, 11, 19, 14, 26, 41, 801, DateTimeKind.Local).AddTicks(1482) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 5,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2020, 11, 19, 14, 26, 41, 801, DateTimeKind.Local).AddTicks(1533), new DateTime(2020, 11, 19, 14, 26, 41, 801, DateTimeKind.Local).AddTicks(1541) });

            migrationBuilder.AddForeignKey(
                name: "FK_Position_Observation_ObservationId",
                table: "Position",
                column: "ObservationId",
                principalTable: "Observation",
                principalColumn: "ObservationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
