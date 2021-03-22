using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Birder.Migrations
{
    public partial class indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 1,
                column: "LastUpdateDate",
                value: new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143));

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 2,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143), new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 3,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143), new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 4,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143), new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 5,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143), new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143) });

            migrationBuilder.CreateIndex(
                name: "IX_Observation_ObservationDateTime",
                table: "Observation",
                column: "ObservationDateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Observation_ObservationDateTime",
                table: "Observation");

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 1,
                column: "LastUpdateDate",
                value: new DateTime(2021, 2, 16, 21, 18, 37, 759, DateTimeKind.Local).AddTicks(8127));

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 2,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 2, 16, 21, 18, 37, 761, DateTimeKind.Local).AddTicks(4114), new DateTime(2021, 2, 16, 21, 18, 37, 761, DateTimeKind.Local).AddTicks(4160) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 3,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 2, 16, 21, 18, 37, 761, DateTimeKind.Local).AddTicks(4320), new DateTime(2021, 2, 16, 21, 18, 37, 761, DateTimeKind.Local).AddTicks(4325) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 4,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 2, 16, 21, 18, 37, 761, DateTimeKind.Local).AddTicks(4356), new DateTime(2021, 2, 16, 21, 18, 37, 761, DateTimeKind.Local).AddTicks(4360) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 5,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 2, 16, 21, 18, 37, 761, DateTimeKind.Local).AddTicks(4386), new DateTime(2021, 2, 16, 21, 18, 37, 761, DateTimeKind.Local).AddTicks(4390) });
        }
    }
}
