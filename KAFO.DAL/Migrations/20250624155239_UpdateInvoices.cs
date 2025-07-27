using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KAFO.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_CustomerAccounts_CustomerAccountId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Invoices");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_CustomerAccounts_CustomerAccountId",
                table: "Invoices",
                column: "CustomerAccountId",
                principalTable: "CustomerAccounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_CustomerAccounts_CustomerAccountId",
                table: "Invoices");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Invoices",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_CustomerAccounts_CustomerAccountId",
                table: "Invoices",
                column: "CustomerAccountId",
                principalTable: "CustomerAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
