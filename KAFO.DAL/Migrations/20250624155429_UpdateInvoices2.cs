using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KAFO.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoices2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_CustomerAccounts_CustomerAccountId",
                table: "Invoices");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerAccountId",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_CustomerAccounts_CustomerAccountId",
                table: "Invoices",
                column: "CustomerAccountId",
                principalTable: "CustomerAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_CustomerAccounts_CustomerAccountId",
                table: "Invoices");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerAccountId",
                table: "Invoices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_CustomerAccounts_CustomerAccountId",
                table: "Invoices",
                column: "CustomerAccountId",
                principalTable: "CustomerAccounts",
                principalColumn: "Id");
        }
    }
}
