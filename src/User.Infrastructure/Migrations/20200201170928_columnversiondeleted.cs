using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace User.Infrastructure.Migrations
{
    public partial class columnversiondeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Users",
                type: "bytea",
                maxLength: 8,
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[] {  });
        }
    }
}
