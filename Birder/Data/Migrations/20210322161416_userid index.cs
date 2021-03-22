using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Birder.Migrations
{
    public partial class useridindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 1,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 3, 22, 16, 14, 14, 990, DateTimeKind.Local).AddTicks(4104), new DateTime(2021, 3, 22, 16, 14, 15, 24, DateTimeKind.Local).AddTicks(4868) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 2,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 3, 22, 16, 14, 15, 26, DateTimeKind.Local).AddTicks(2673), new DateTime(2021, 3, 22, 16, 14, 15, 26, DateTimeKind.Local).AddTicks(2721) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 3,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 3, 22, 16, 14, 15, 26, DateTimeKind.Local).AddTicks(2876), new DateTime(2021, 3, 22, 16, 14, 15, 26, DateTimeKind.Local).AddTicks(2882) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 4,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 3, 22, 16, 14, 15, 26, DateTimeKind.Local).AddTicks(2911), new DateTime(2021, 3, 22, 16, 14, 15, 26, DateTimeKind.Local).AddTicks(2916) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 5,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2021, 3, 22, 16, 14, 15, 26, DateTimeKind.Local).AddTicks(2941), new DateTime(2021, 3, 22, 16, 14, 15, 26, DateTimeKind.Local).AddTicks(2945) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
