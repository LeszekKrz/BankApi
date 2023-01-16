using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEnumTypesToClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GovIdType",
                table: "Offers",
                newName: "Income");

            migrationBuilder.RenameColumn(
                name: "GovId",
                table: "Offers",
                newName: "GovernmentIdValue");

            migrationBuilder.AddColumn<string>(
                name: "GovernmentIdName",
                table: "Offers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GovernmentIdTypeId",
                table: "Offers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GovernmentIdName",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "GovernmentIdTypeId",
                table: "Offers");

            migrationBuilder.RenameColumn(
                name: "Income",
                table: "Offers",
                newName: "GovIdType");

            migrationBuilder.RenameColumn(
                name: "GovernmentIdValue",
                table: "Offers",
                newName: "GovId");
        }
    }
}
