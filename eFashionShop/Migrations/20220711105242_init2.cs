using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eFashionShop.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d9c1a882-46da-4802-8e88-bf4e10de3437");

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4eac54b6-e019-4009-897a-47cbc41e44a0", "AQAAAAEAACcQAAAAEADV/hJoYxSnWipOTYuo36FT2nLmxnFRfvts/Ar5Geg/VM+VDNI2dajDjC9GFMog6A==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppRole",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7cda7f20-09ab-45af-ab89-f798bbc03276");

            migrationBuilder.UpdateData(
                table: "AppUser",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1c8776e5-6293-4cc2-8741-d74f2c360506", "AQAAAAEAACcQAAAAEDLQH2HYmPYzILpX+6fUVg2VjG/S8tpFBgtd8Mnxis6xoQR20a268GYyL3SAyAXDog==" });
        }
    }
}
