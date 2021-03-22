using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Birder.Migrations
{
    public partial class obsersationentityindexesadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 1,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 3, 22, 15, 41, 31, 669, DateTimeKind.Local).AddTicks(1996), new DateTime(2021, 3, 22, 15, 41, 31, 672, DateTimeKind.Local).AddTicks(9815) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 2,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 3, 22, 15, 41, 31, 674, DateTimeKind.Local).AddTicks(5814), new DateTime(2021, 3, 22, 15, 41, 31, 674, DateTimeKind.Local).AddTicks(5853) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 3,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 3, 22, 15, 41, 31, 674, DateTimeKind.Local).AddTicks(5996), new DateTime(2021, 3, 22, 15, 41, 31, 674, DateTimeKind.Local).AddTicks(6001) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 4,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 3, 22, 15, 41, 31, 674, DateTimeKind.Local).AddTicks(6025), new DateTime(2021, 3, 22, 15, 41, 31, 674, DateTimeKind.Local).AddTicks(6029) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 5,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 3, 22, 15, 41, 31, 674, DateTimeKind.Local).AddTicks(6054), new DateTime(2021, 3, 22, 15, 41, 31, 674, DateTimeKind.Local).AddTicks(6058) });

            migrationBuilder.CreateIndex(
                name: "IX_Observation_ObservationDateTime",
                table: "Observation",
                column: "ObservationDateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Observation_SelectedPrivacyLevel",
                table: "Observation",
                column: "SelectedPrivacyLevel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Observation_ObservationDateTime",
                table: "Observation");

            migrationBuilder.DropIndex(
                name: "IX_Observation_SelectedPrivacyLevel",
                table: "Observation");

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 1,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 2, 16, 21, 18, 37, 756, DateTimeKind.Local).AddTicks(2143), new DateTime(2021, 2, 16, 21, 18, 37, 759, DateTimeKind.Local).AddTicks(8127) });

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
