using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataModels.Migrations
{
    /// <inheritdoc />
    public partial class m2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "c6647262-ef40-40b3-af33-f89f80d35378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d10c6b18-97cb-4cdc-879d-d523ea539567", "AQAAAAIAAYagAAAAEHafy3xl6btjLkVgMJ/151ZKbkYboD/cA1OV1YfiptwjxqHMtjFV7vJVXwNiE7JXvw==", "d77c3e6b-70d2-4718-9df6-a72c8090aa9a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "c6647262-ef40-40b3-af33-f89f80d35378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "689e3814-6e89-4a6e-95aa-673125ab2d96", "AQAAAAIAAYagAAAAEPUtNX0xFIFFgu+vPPY1GYESJ0UmaTDP9cSRg0UqWW2vuJ6oAf810aC3sV7EWY9vRw==", "8709371a-d4e6-4649-8f93-28af2ecc795b" });
        }
    }
}
