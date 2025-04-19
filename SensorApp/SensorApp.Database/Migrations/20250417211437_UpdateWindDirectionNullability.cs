using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SensorApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWindDirectionNullability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1243c642-7fdf-4224-9404-02dd6ac95bc5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c8e257a0-c263-4618-8165-03c1e37231b5", "AQAAAAIAAYagAAAAEJX8iZYLUkzOhAQoEmunL4GgRKaXmWkrneyzNy4/u3NiVlg4mpbTIOCmHKCpgPCCKA==", "da123e81-674e-4326-ad01-23ea506816d9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "99166c0c-7f14-442b-8c57-9141f3ac1681",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "01308dd3-243e-4975-b9c0-bb8acf051a53", "AQAAAAIAAYagAAAAEEcWRjC6krMJzZ5zzf/PV9Hxl4nr3MHalKnWp44BA9w597rJcST7xviGJXZprak3dA==", "aa0573e1-547a-4aa2-830c-1a0725634c99" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fab66dad-9f12-45a0-9fd8-6352336a696d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4aa2d158-19cf-4e45-a7e8-f97658a15dc8", "AQAAAAIAAYagAAAAEBivwc3EI6LpXwM5xxAki6dVB+FJ7vWDvslvV0Qz1spQAzH8ITdlD8seqmhTVgaEow==", "bace8073-027b-456f-898b-539781935346" });
            migrationBuilder.UpdateData(
                table: "measurand",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Max_safe_threshold", "Min_safe_threshold" },
                values: new object[] { null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1243c642-7fdf-4224-9404-02dd6ac95bc5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d0983d68-7dfd-4ee2-8a95-2ba821de52c1", "AQAAAAIAAYagAAAAEFSPpIgdFHXWzQbItGeqBgObzksNRRNh/KJOgsMwkF8lVq7jlsE08XSE3A6qaEPGIg==", "b16b9dd7-542f-4f8f-aa78-ff9f03c14acf" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "99166c0c-7f14-442b-8c57-9141f3ac1681",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4f46dbd4-f2ff-425a-a45b-7334b619723e", "AQAAAAIAAYagAAAAELTZPlFMpsz4Sc5fZRPbIjUXzbkQt2Hqr/fjFRLtKn3Kx6gm1bF4vMPK/7S+AO+R6g==", "eff6b1c2-fda0-4bde-80a3-fd1ce4707eac" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fab66dad-9f12-45a0-9fd8-6352336a696d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bc04de79-54f6-45f1-8e19-977338435cf4", "AQAAAAIAAYagAAAAEOeDmu4XimclZLiHHCn7CeupJuz+0iBI1uLA7oJF9K/RU04VPhkCEvRRvkLbUjdPWg==", "00e455f9-85fc-4758-8261-5e4633a82bfd" });
        }
    }
}
