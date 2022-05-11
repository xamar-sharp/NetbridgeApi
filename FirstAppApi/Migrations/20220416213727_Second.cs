using Microsoft.EntityFrameworkCore.Migrations;

namespace FirstAppApi.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsExist",
                table: "Friends",
                newName: "IsActive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Friends",
                newName: "IsExist");
        }
    }
}
