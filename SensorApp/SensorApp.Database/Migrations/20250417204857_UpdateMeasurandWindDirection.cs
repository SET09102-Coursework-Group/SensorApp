using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SensorApp.Database.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMeasurandWindDirection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Min_safe_threshold",
                table: "measurand",
                type: "REAL",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "REAL");

            migrationBuilder.AlterColumn<float>(
                name: "Max_safe_threshold",
                table: "measurand",
                type: "REAL",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "REAL");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1243c642-7fdf-4224-9404-02dd6ac95bc5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8566ac49-19e8-474a-9a27-b9c572589975", "AQAAAAIAAYagAAAAEN3DtfxFQoWGNA1VZERRgVO3NeOrcDZXpit9RVqckFowh21ZJoNAPkTJCo3nUJMRUg==", "25812c88-f1e1-4e5c-b6dd-989fb144f8ed" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "99166c0c-7f14-442b-8c57-9141f3ac1681",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e7207acf-ac3f-4499-ab92-4a5f58203cc6", "AQAAAAIAAYagAAAAEDFsFHSg1k66m7Hb5Bp+4Pbzj2VEOGZ8QqParu0zfh/4UDqwfCae9lkDrbTGtTkB3g==", "217cd567-a6f2-40dc-8fb7-74aedd7ab418" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "fab66dad-9f12-45a0-9fd8-6352336a696d",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c51a027f-0fa6-4a17-ae48-ca5ee78f245e", "AQAAAAIAAYagAAAAEPKmWqpAIHdZPEQfggxYSJegbqfE22O9TDANMFD1IDud2UFriMVzyYhr64+DVFrIQA==", "1c33bfef-8459-4389-91de-00dc66ba6d18" });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Min_safe_threshold",
                table: "measurand",
                type: "REAL",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "REAL",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Max_safe_threshold",
                table: "measurand",
                type: "REAL",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "REAL",
                oldNullable: true);

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
        }
    }
}
