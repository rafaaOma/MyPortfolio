using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProtfolio.API.Migrations
{
    /// <inheritdoc />
    public partial class rafaainfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Password", "UserName" },
                values: new object[] { 1, "$2a$11$yH99rJMZqF951xuZHA0MdeZlYwf4CIPQ3UG1W8fU5lRBjI9PVlfv6", "rafa" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
