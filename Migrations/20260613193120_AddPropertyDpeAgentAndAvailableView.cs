using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YmmoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyDpeAgentAndAvailableView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgentId",
                table: "Properties",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DpeRating",
                table: "Properties",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AgentId", "CreatedAt", "DpeRating" },
                values: new object[] { null, new DateTime(2026, 6, 13, 19, 31, 19, 550, DateTimeKind.Utc).AddTicks(2249), null });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AgentId", "CreatedAt", "DpeRating" },
                values: new object[] { null, new DateTime(2026, 6, 13, 19, 31, 19, 550, DateTimeKind.Utc).AddTicks(8081), null });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AgentId", "CreatedAt", "DpeRating" },
                values: new object[] { null, new DateTime(2026, 6, 13, 19, 31, 19, 550, DateTimeKind.Utc).AddTicks(8088), null });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AgentId", "CreatedAt", "DpeRating" },
                values: new object[] { null, new DateTime(2026, 6, 13, 19, 31, 19, 550, DateTimeKind.Utc).AddTicks(8092), null });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AgentId", "CreatedAt", "DpeRating" },
                values: new object[] { null, new DateTime(2026, 6, 13, 19, 31, 19, 550, DateTimeKind.Utc).AddTicks(8095), null });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AgentId", "CreatedAt", "DpeRating" },
                values: new object[] { null, new DateTime(2026, 6, 13, 19, 31, 19, 550, DateTimeKind.Utc).AddTicks(8098), null });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "AgentId", "CreatedAt", "DpeRating" },
                values: new object[] { null, new DateTime(2026, 6, 13, 19, 31, 19, 550, DateTimeKind.Utc).AddTicks(8100), null });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AgentId", "CreatedAt", "DpeRating" },
                values: new object[] { null, new DateTime(2026, 6, 13, 19, 31, 19, 550, DateTimeKind.Utc).AddTicks(8104), null });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "AgentId", "CreatedAt", "DpeRating" },
                values: new object[] { null, new DateTime(2026, 6, 13, 19, 31, 19, 550, DateTimeKind.Utc).AddTicks(8106), null });

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "AgentId", "CreatedAt", "DpeRating" },
                values: new object[] { null, new DateTime(2026, 6, 13, 19, 31, 19, 550, DateTimeKind.Utc).AddTicks(8109), null });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_AgentId",
                table: "Properties",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Users_AgentId",
                table: "Properties",
                column: "AgentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.Sql("""
                CREATE OR REPLACE VIEW vw_biens_disponibles AS
                SELECT
                    p."Id"          AS "PropertyId",
                    p."Title",
                    p."Type",
                    p."Price",
                    p."City",
                    p."Bedrooms",
                    p."Area",
                    p."DpeRating",
                    p."ListedDate",
                    a."Id"          AS "AgencyId",
                    a."Name"        AS "AgencyName",
                    a."City"        AS "AgencyCity"
                FROM "Properties" p
                JOIN "Agencies" a ON a."Id" = p."AgencyId"
                WHERE p."Status" = 'Available';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Users_AgentId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_AgentId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "DpeRating",
                table: "Properties");

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 12, 16, 11, 33, 793, DateTimeKind.Utc).AddTicks(8865));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 12, 16, 11, 33, 794, DateTimeKind.Utc).AddTicks(3106));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 12, 16, 11, 33, 794, DateTimeKind.Utc).AddTicks(3112));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 12, 16, 11, 33, 794, DateTimeKind.Utc).AddTicks(3115));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 12, 16, 11, 33, 794, DateTimeKind.Utc).AddTicks(3121));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 12, 16, 11, 33, 794, DateTimeKind.Utc).AddTicks(3123));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 12, 16, 11, 33, 794, DateTimeKind.Utc).AddTicks(3125));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 12, 16, 11, 33, 794, DateTimeKind.Utc).AddTicks(3128));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 12, 16, 11, 33, 794, DateTimeKind.Utc).AddTicks(3130));

            migrationBuilder.UpdateData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 12, 16, 11, 33, 794, DateTimeKind.Utc).AddTicks(3150));
        }
    }
}
