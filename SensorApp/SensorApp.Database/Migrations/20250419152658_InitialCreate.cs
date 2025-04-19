using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SensorApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    { "1243c642-7fdf-4224-9404-02dd6ac95bc5", 0, "36d7a5a3-1325-427a-ad20-92176abb1223", "scientist@sensor.com", true, false, null, "SCIENTIST@SENSOR.COM", "SCIENTIST@SENSOR.COM", "AQAAAAIAAYagAAAAEFDOJ/6yRwp00tD09KMHXd8P7spUCMDF1Kjk+zVTH3nXWcRL/zxbq9RSicP7Mo120A==", null, false, "69c996b6-a6a7-4224-8b3e-c3980eebbed9", false, "scientist@sensor.com" },
                    { "99166c0c-7f14-442b-8c57-9141f3ac1681", 0, "c3e79e40-9fa5-44fe-ba05-585b1b97c815", "ops@sensor.com", true, false, null, "OPS@SENSOR.COM", "OPS@SENSOR.COM", "AQAAAAIAAYagAAAAEK87KLp0OuA3vVuh2jFy6qbO3C+SeW8Sg1zZrP441aIVcQscwjH1MzDTuxDTWDY0sA==", null, false, "50f9fb4a-065e-4607-8565-05e67c592d1b", false, "ops@sensor.com" },
                    { "fab66dad-9f12-45a0-9fd8-6352336a696d", 0, "15981a80-23cd-4753-a50a-eab86efe50da", "admin@sensor.com", true, false, null, "ADMIN@SENSOR.COM", "ADMIN@SENSOR.COM", "AQAAAAIAAYagAAAAEHOYByGEL9lcq0I6wAzzFLvp4VIeNMe2YTbp4HWKvlb3NAfX5D/KjdLkLvekRrtCyQ==", null, false, "29d2867c-fa39-4f83-b5d3-11cd87eef188", false, "admin@sensor.com" }
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

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "9b7f193f-bfc4-4eb7-927f-55960e45a82a", "1243c642-7fdf-4224-9404-02dd6ac95bc5" },
                    { "71136dd8-0a29-4d9a-b3fe-bd176ba7aa9c", "99166c0c-7f14-442b-8c57-9141f3ac1681" },
                    { "3d52c1e5-6aec-45de-91c1-e0ebf20464e3", "fab66dad-9f12-45a0-9fd8-6352336a696d" }
                });

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
