using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eFashionShop.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c2eaeec5-24aa-4f0a-bd8e-e7d01c89e78e");

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "92b4efb2-caf0-4a5d-aaf1-e3c57f9d2eb3", "AQAAAAEAACcQAAAAENWKJNj7mft22XQOaRol9NZIwiNXnyVep4yPf3ByJVT1v3cQOxfG/1Alh7/HYHFc8A==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ca088432-34d5-4a9e-8fd4-2f45ab0d81bd");

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d2c85753-ff5c-4f37-be48-5e0385ea96a0", "AQAAAAEAACcQAAAAEORkhJEvcOdtD2zikrReANMPvI5fqN1Dlt7oj627oZKFhBVkv2ZSyjf5UjGI4yoDpw==" });
        }
    }
}
