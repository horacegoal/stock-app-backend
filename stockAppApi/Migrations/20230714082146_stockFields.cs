using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stockAppApi.Migrations
{
    /// <inheritdoc />
    public partial class stockFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "High",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "Low",
                table: "Stocks");

            migrationBuilder.RenameColumn(
                name: "Volume",
                table: "Stocks",
                newName: "PERatio");

            migrationBuilder.RenameColumn(
                name: "Open",
                table: "Stocks",
                newName: "MarketCap");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PERatio",
                table: "Stocks",
                newName: "Volume");

            migrationBuilder.RenameColumn(
                name: "MarketCap",
                table: "Stocks",
                newName: "Open");

            migrationBuilder.AddColumn<decimal>(
                name: "High",
                table: "Stocks",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Low",
                table: "Stocks",
                type: "TEXT",
                nullable: true);
        }
    }
}
