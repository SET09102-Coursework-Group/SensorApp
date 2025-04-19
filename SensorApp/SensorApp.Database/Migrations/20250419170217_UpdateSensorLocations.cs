using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SensorApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSensorLocations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1243c642-7fdf-4224-9404-02dd6ac95bc5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9f919890-fdd6-4d93-b3c9-090af55ba9fe", "AQAAAAIAAYagAAAAEJMKp5/DAxTWAEe9wTFSu8YQPR37GzyW3O2Qti2uMSteTDEgH6L3kOiZI28s4wWW2w==", "f965b445-c33c-4a72-93ba-79c3598bb5ce" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "99166c0c-7f14-442b-8c57-9141f3ac1681",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a15c16a0-5881-440d-9ddc-3fb21d1d5685", "AQAAAAIAAYagAAAAEK8UA0x16WCUc32tqv+Wy25guN6PlTAyV8MgOrqFdVkbtcAYr/cts6uuG+Nqa5BcFQ==", "52d35023-32fc-4ad6-a102-19aa70375f32" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fab66dad-9f12-45a0-9fd8-6352336a696d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d9eaf8e4-18d2-432d-88d9-ddb82023fd4e", "AQAAAAIAAYagAAAAEMEkbjzrT7Lp8A6YEFZvwyd+jjMKQGEMZiQ5J6ypk33FWhPa2FwgUeAUpSekA05WLQ==", "78671824-9b5d-4194-9292-0f02b5edbdc0" });

            migrationBuilder.UpdateData(
                table: "sensor",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 55.94476f, -3.183991f });

            migrationBuilder.UpdateData(
                table: "sensor",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 55.86111f, -3.253889f });

            migrationBuilder.UpdateData(
                table: "sensor",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 55.008785f, -3.5856323f });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                table: "sensor",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { -3.183991f, 55.94476f });

            migrationBuilder.UpdateData(
                table: "sensor",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { -3.253889f, 55.86111f });

            migrationBuilder.UpdateData(
                table: "sensor",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { -3.5856323f, 55.008785f });
        }
    }
}
