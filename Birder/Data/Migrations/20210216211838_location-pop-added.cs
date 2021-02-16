using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Birder.Migrations
{
    public partial class locationpopadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortAddress",
                table: "ObservationPosition",
                type: "nvarchar(max)",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortAddress",
                table: "ObservationPosition");

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 1,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2020, 11, 27, 21, 57, 3, 84, DateTimeKind.Local).AddTicks(1616), new DateTime(2020, 11, 27, 21, 57, 3, 87, DateTimeKind.Local).AddTicks(1340) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 2,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2020, 11, 27, 21, 57, 3, 88, DateTimeKind.Local).AddTicks(5446), new DateTime(2020, 11, 27, 21, 57, 3, 88, DateTimeKind.Local).AddTicks(5473) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 3,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2020, 11, 27, 21, 57, 3, 88, DateTimeKind.Local).AddTicks(5594), new DateTime(2020, 11, 27, 21, 57, 3, 88, DateTimeKind.Local).AddTicks(5599) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 4,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2020, 11, 27, 21, 57, 3, 88, DateTimeKind.Local).AddTicks(5622), new DateTime(2020, 11, 27, 21, 57, 3, 88, DateTimeKind.Local).AddTicks(5625) });

            migrationBuilder.UpdateData(
                table: "ConservationStatus",
                keyColumn: "ConservationStatusId",
                keyValue: 5,
                columns: new[] { "CreationDate", "LastUpdateDate" },
                values: new object[] { new DateTime(2020, 11, 27, 21, 57, 3, 88, DateTimeKind.Local).AddTicks(5645), new DateTime(2020, 11, 27, 21, 57, 3, 88, DateTimeKind.Local).AddTicks(5649) });
        }
    }
}
