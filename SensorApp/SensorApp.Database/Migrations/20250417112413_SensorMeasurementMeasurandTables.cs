using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SensorApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class SensorMeasurementMeasurandTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "measurand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    Min_safe_threshold = table.Column<float>(type: "REAL", nullable: false),
                    Max_safe_threshold = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_measurand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sensor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Longitude = table.Column<float>(type: "REAL", nullable: false),
                    Latitude = table.Column<float>(type: "REAL", nullable: false),
                    Site_zone = table.Column<string>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sensor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "measurement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Value = table.Column<float>(type: "REAL", nullable: false),
                    Sensor_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Measurement_type_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_measurement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_measurement_measurand_Measurement_type_id",
                        column: x => x.Measurement_type_id,
                        principalTable: "measurand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_measurement_sensor_Sensor_id",
                        column: x => x.Sensor_id,
                        principalTable: "sensor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1243c642-7fdf-4224-9404-02dd6ac95bc5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "53a30203-0298-4f46-ad56-e2e039a48249", "AQAAAAIAAYagAAAAENP+FNMMjxWfiProMscLEw4/onxAsbDwfWvekwMi4fHo+x49eRE0TdtULrQoXLJ/Jg==", "e51cefbd-2261-4919-8664-8c0b10a59923" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "99166c0c-7f14-442b-8c57-9141f3ac1681",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "caef29bf-6ed6-4bd7-8a6c-70e31426894a", "AQAAAAIAAYagAAAAEHKnYfJIW39H44YuAZYIjvwoTZZOWMQjJMB30v2LoKFI5FKm6mn9ztWWZeP9DiGWAA==", "b678bdcc-d5ab-46b0-a21a-ef650cb67115" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fab66dad-9f12-45a0-9fd8-6352336a696d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "631ba4d4-763f-482c-8b39-84d9b095b4b8", "AQAAAAIAAYagAAAAEGcVLUZHCBSuD+pJ2Fjuaas5gsGwahjlJ2SF5ST8zJI+Kcf/cXkbzXNFU1hxSboqVQ==", "d510fee3-7b9c-4558-8169-aafe69b3e00b" });

            migrationBuilder.InsertData(
                table: "measurand",
                columns: new[] { "Id", "Max_safe_threshold", "Min_safe_threshold", "Name", "Unit" },
                values: new object[,]
                {
                    { 1, 50f, 10f, "Nitrogen Dioxide", "ug/m3" },
                    { 2, 2f, 0.8f, "Sulphur Dioxide", "ug/m3" },
                    { 3, 20f, 1.5f, "PM2.5 Particulate Matter", "ug/m3" },
                    { 4, 12f, 2f, "PM10 Particulate Matter", "ug/m3" },
                    { 5, 25f, 20f, "Nitrate", "mg/l" },
                    { 6, 1.5f, 1.1f, "Nitrite", "mg/l" },
                    { 7, 0.07f, 0.02f, "Phosphate", "mg/l" },
                    { 8, 0.1f, 0f, "Escherichia coli", "cfu/100ml" },
                    { 9, 40f, -10f, "Temperature", "C" },
                    { 10, 100f, 80f, "Relative Humidity", "%" },
                    { 11, 30f, 0f, "Wind Speed", "m/s" },
                    { 12, 0f, 0f, "Wind Direction", "degree" }
                });

            migrationBuilder.InsertData(
                table: "sensor",
                columns: new[] { "Id", "Latitude", "Longitude", "Site_zone", "Status", "Type" },
                values: new object[,]
                {
                    { 1, -3.183991f, 55.94476f, "Central Scotland", "Active", "Air Quality" },
                    { 2, -3.253889f, 55.86111f, "Glencorse B", "Active", "Water Quality" },
                    { 3, -3.5856323f, 55.008785f, null, "Active", "Weather" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_measurement_Measurement_type_id",
                table: "measurement",
                column: "Measurement_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_measurement_Sensor_id",
                table: "measurement",
                column: "Sensor_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "measurement");

            migrationBuilder.DropTable(
                name: "measurand");

            migrationBuilder.DropTable(
                name: "sensor");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1243c642-7fdf-4224-9404-02dd6ac95bc5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "db3bd552-6112-47f6-8872-6efad06eb18c", "AQAAAAIAAYagAAAAEMWF6Eof6SHzlePtb6IbUm/EL1NgXWMwUqkbWIqr8dJg1sBqhXsVKDGJzjdubDUsNg==", "3ae8d5a3-478b-430b-b9a9-d54dbe4c6b74" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "99166c0c-7f14-442b-8c57-9141f3ac1681",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5b00f504-39e6-4d1e-8e2f-6a9edfebff46", "AQAAAAIAAYagAAAAED2nWQOVWgVcKPo5c79Jn3RtbM3deAj+FDc9/Txe715xDu3frePfB/FAlmyWtgaN8A==", "19284033-5c81-417b-b58b-7b630d87b498" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fab66dad-9f12-45a0-9fd8-6352336a696d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "37db67b8-4faa-49af-bdf3-4f1f261617cf", "AQAAAAIAAYagAAAAEGoxx4Sj1qyGk/0260HWndmE0yxg2n+/dRzSsR76mZYNd8yFM5hDzvbnAMZI8xDpsQ==", "2545e6c4-13aa-42f4-8128-d07376fa6281" });
        }
    }
}
