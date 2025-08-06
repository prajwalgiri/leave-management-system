using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AttendenceSystem.Migrations
{
    /// <inheritdoc />
    public partial class LeavePoliciy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeavePolicies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AllowedLeaves = table.Column<int>(type: "INTEGER", nullable: false),
                    FromDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ToDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeavePolicies", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "106acfe0-d607-4b83-8b76-4dc39869c769", "AQAAAAIAAYagAAAAEOPPa7GbI0/jvZ4Ly+Dk2no6JBvyfrjqJrg0An+AJZZk34S4mOemjPQzrGMdEwTYew==", "4a551029-83bc-4626-aeab-8e4338ebe232" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeavePolicies");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0d03deec-34f6-42d1-8099-19b0c8836cd7", "AQAAAAIAAYagAAAAELr0dcG2rwCH6xoMXxaOu6d09t65CR0D9t1VWsMph08bZhVnh2ZZoyFugpxShX2YyQ==", "c7210ac1-87ec-4bbb-9658-887718af71f8" });
        }
    }
}
