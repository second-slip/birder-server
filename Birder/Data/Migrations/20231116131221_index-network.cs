using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Birder.Migrations
{
    /// <inheritdoc />
    public partial class indexnetwork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Network",
                table: "Network");

            migrationBuilder.AlterColumn<string>(
                name: "FollowerId",
                table: "Network",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Network",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "NetworkId",
                table: "Network",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Network",
                table: "Network",
                column: "NetworkId");

            migrationBuilder.CreateIndex(
                name: "IX_Network_ApplicationUserId_FollowerId",
                table: "Network",
                columns: new[] { "ApplicationUserId", "FollowerId" },
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL AND [FollowerId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Network",
                table: "Network");

            migrationBuilder.DropIndex(
                name: "IX_Network_ApplicationUserId_FollowerId",
                table: "Network");

            migrationBuilder.DropColumn(
                name: "NetworkId",
                table: "Network");

            migrationBuilder.AlterColumn<string>(
                name: "FollowerId",
                table: "Network",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Network",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Network",
                table: "Network",
                columns: new[] { "ApplicationUserId", "FollowerId" });
        }
    }
}
