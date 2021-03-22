using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Birder.Migrations
{
    public partial class obsersationentityindexremoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Observation_SelectedPrivacyLevel",
                table: "Observation");

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 1,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 3, 22, 15, 51, 40, 455, DateTimeKind.Local).AddTicks(7738), new DateTime(2021, 3, 22, 15, 51, 40, 459, DateTimeKind.Local).AddTicks(4258) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 2,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 3, 22, 15, 51, 40, 460, DateTimeKind.Local).AddTicks(9086), new DateTime(2021, 3, 22, 15, 51, 40, 460, DateTimeKind.Local).AddTicks(9124) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 3,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 3, 22, 15, 51, 40, 460, DateTimeKind.Local).AddTicks(9273), new DateTime(2021, 3, 22, 15, 51, 40, 460, DateTimeKind.Local).AddTicks(9278) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 4,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 3, 22, 15, 51, 40, 460, DateTimeKind.Local).AddTicks(9302), new DateTime(2021, 3, 22, 15, 51, 40, 460, DateTimeKind.Local).AddTicks(9307) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 5,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 3, 22, 15, 51, 40, 460, DateTimeKind.Local).AddTicks(9328), new DateTime(2021, 3, 22, 15, 51, 40, 460, DateTimeKind.Local).AddTicks(9332) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "IX_Observation_SelectedPrivacyLevel",
                table: "Observation",
                column: "SelectedPrivacyLevel");
        }
    }
}
