using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataModels.Migrations
{
    /// <inheritdoc />
    public partial class migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "31234cbd-ca9b-45a2-9028-192615dc638a", null, "Employee", "EMPLOYEE" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "c6647262-ef40-40b3-af33-f89f80d35378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7bb49ed4-e1ac-493f-8204-9d33d9c0c9a3", "AQAAAAIAAYagAAAAEEeCmQLUtnCTfkO4ZanuDcQp2Zjl+PvFRsstZElCZtZrAgbzX/8T2bdjua3M7gFw+Q==", "850fa45d-939c-443d-a4e0-d99aad2b449d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "31234cbd-ca9b-45a2-9028-192615dc638a");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "c6647262-ef40-40b3-af33-f89f80d35378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c764f572-8a8d-4078-b936-50f1c884bde5", "AQAAAAIAAYagAAAAEN8VUxuQh75N5p2/IsApdnt67xUQ3k4P70PvlcY9wuih7P4xxsnDbaJDJqFw5DY9+A==", "94320fc5-3ec8-41fd-bf86-1cf553352104" });
        }
    }
}
