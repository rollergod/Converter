using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryRefreshTokenTime",
                table: "Users",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "TransferDate",
                table: "Transfers",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_MainCurrencyId",
                table: "Accounts",
                column: "MainCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Currencies_MainCurrencyId",
                table: "Accounts",
                column: "MainCurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Currencies_MainCurrencyId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_MainCurrencyId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ExpiryRefreshTokenTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Users");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TransferDate",
                table: "Transfers",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
