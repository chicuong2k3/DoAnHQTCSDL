using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataModels.Migrations
{
    /// <inheritdoc />
    public partial class m : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "c6647262-ef40-40b3-af33-f89f80d35378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "689e3814-6e89-4a6e-95aa-673125ab2d96", "AQAAAAIAAYagAAAAEPUtNX0xFIFFgu+vPPY1GYESJ0UmaTDP9cSRg0UqWW2vuJ6oAf810aC3sV7EWY9vRw==", "8709371a-d4e6-4649-8f93-28af2ecc795b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "c6647262-ef40-40b3-af33-f89f80d35378",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a4af571d-c642-4591-819b-6ac68cf2b2e4", "AQAAAAIAAYagAAAAEOQC9obpi/2t5aJBa/NcyO9HItDhH+xOugsDShH7ohgD2+1nQ+npbBlgKJZv22ygpg==", "1a0109f6-f915-4a4c-9646-c73463d39fee" });
        }
    }
}
