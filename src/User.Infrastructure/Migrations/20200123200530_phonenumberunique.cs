using Microsoft.EntityFrameworkCore.Migrations;

namespace User.Infrastructure.Migrations
{
    public partial class phonenumberunique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_FirstName",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullPhoneNumber",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FullPhoneNumber",
                table: "Users",
                column: "FullPhoneNumber",
                unique: true,
                filter: "[FullPhoneNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_FullPhoneNumber",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FullPhoneNumber",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FirstName",
                table: "Users",
                column: "FirstName",
                unique: true,
                filter: "[FirstName] IS NOT NULL");
        }
    }
}
