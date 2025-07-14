using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KAFO.DAL.Migrations
{
    /// <inheritdoc />
    public partial class updateCrediteCustomerData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "CustomerAccounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "CustomerAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "CustomerAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "CustomerAccounts");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "CustomerAccounts");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "CustomerAccounts");
        }
    }
}
