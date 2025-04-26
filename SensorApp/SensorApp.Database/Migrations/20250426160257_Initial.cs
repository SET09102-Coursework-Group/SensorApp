using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SensorApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "measurand",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", nullable: false),
                    Min_safe_threshold = table.Column<float>(type: "REAL", nullable: true),
                    Max_safe_threshold = table.Column<float>(type: "REAL", nullable: true)
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "incident",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Sensor_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Creation_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Priority = table.Column<int>(type: "INTEGER", nullable: false),
                    Resolution_date = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Responder_id = table.Column<string>(type: "TEXT", nullable: false),
                    Comments = table.Column<string>(type: "TEXT", nullable: true),
                    Resolution_comments = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_incident", x => x.Id);
                    table.ForeignKey(
                        name: "FK_incident_AspNetUsers_Responder_id",
                        column: x => x.Responder_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_incident_sensor_Sensor_id",
                        column: x => x.Sensor_id,
                        principalTable: "sensor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3d52c1e5-6aec-45de-91c1-e0ebf20464e3", null, "Administrator", "ADMINISTRATOR" },
                    { "71136dd8-0a29-4d9a-b3fe-bd176ba7aa9c", null, "OperationsManager", "OPERATIONSMANAGER" },
                    { "9b7f193f-bfc4-4eb7-927f-55960e45a82a", null, "EnvironmentalScientist", "ENVIRONMENTALSCIENTIST" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1243c642-7fdf-4224-9404-02dd6ac95bc5", 0, "e4d12d3d-802a-4a20-8c47-1c8de1876c95", "scientist@sensor.com", true, false, null, "SCIENTIST@SENSOR.COM", "SCIENTIST@SENSOR.COM", "AQAAAAIAAYagAAAAEFlCyRbp/V2qv4xNr72PbdkHCKwszvEIqi14gzmtRNfAV+RuUfa/hBrP8Z1n8LFyQQ==", null, false, "c111d98e-3c22-4072-ae56-46aa570cb63d", false, "scientist@sensor.com" },
                    { "99166c0c-7f14-442b-8c57-9141f3ac1681", 0, "b93b33dc-4e39-4b51-8721-17c2b43ee4ce", "ops@sensor.com", true, false, null, "OPS@SENSOR.COM", "OPS@SENSOR.COM", "AQAAAAIAAYagAAAAEM+j3vhfqqviLkCCTbbctsDYIC4B3HjkGVPIOHSMniHn3aqmJUymv3TRcoeV7Mk/wg==", null, false, "02074b9f-6a32-4134-a034-699fbce99cdf", false, "ops@sensor.com" },
                    { "fab66dad-9f12-45a0-9fd8-6352336a696d", 0, "695efb88-0b87-4d7e-9439-833dfc52b41a", "admin@sensor.com", true, false, null, "ADMIN@SENSOR.COM", "ADMIN@SENSOR.COM", "AQAAAAIAAYagAAAAELa9LUUz8hrHzOXsyAyA/nEcL4IixQuDFHaeyoF6rIrTQT6+N44SMygjpY04DCJmRw==", null, false, "c2fa6f41-3914-4729-bfcf-2061b66b82f9", false, "admin@sensor.com" }
                });

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
                    { 12, null, null, "Wind Direction", "degree" }
                });

            migrationBuilder.InsertData(
                table: "sensor",
                columns: new[] { "Id", "Latitude", "Longitude", "Site_zone", "Status", "Type" },
                values: new object[,]
                {
                    { 1, 55.94476f, -3.183991f, "Central Scotland", "Active", "Air Quality" },
                    { 2, 55.86111f, -3.253889f, "Glencorse B", "Active", "Water Quality" },
                    { 3, 55.008785f, -3.5856323f, null, "Active", "Weather" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "9b7f193f-bfc4-4eb7-927f-55960e45a82a", "1243c642-7fdf-4224-9404-02dd6ac95bc5" },
                    { "71136dd8-0a29-4d9a-b3fe-bd176ba7aa9c", "99166c0c-7f14-442b-8c57-9141f3ac1681" },
                    { "3d52c1e5-6aec-45de-91c1-e0ebf20464e3", "fab66dad-9f12-45a0-9fd8-6352336a696d" }
                });

            migrationBuilder.InsertData(
                table: "incident",
                columns: new[] { "Id", "Comments", "Creation_date", "Priority", "Resolution_comments", "Resolution_date", "Responder_id", "Sensor_id", "Status", "Type" },
                values: new object[] { 1, "Max threshold breach for Nitrogen Dioxide", new DateTime(2025, 4, 26, 16, 2, 57, 382, DateTimeKind.Utc).AddTicks(2712), 0, null, null, "99166c0c-7f14-442b-8c57-9141f3ac1681", 1, 0, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_incident_Responder_id",
                table: "incident",
                column: "Responder_id");

            migrationBuilder.CreateIndex(
                name: "IX_incident_Sensor_id",
                table: "incident",
                column: "Sensor_id");

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "incident");

            migrationBuilder.DropTable(
                name: "measurement");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "measurand");

            migrationBuilder.DropTable(
                name: "sensor");
        }
    }
}
