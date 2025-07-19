using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KAFO.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addInvoiceImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Invoices",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Invoices");
        }
    }
}
