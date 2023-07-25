using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stockAppApi.Migrations
{
    /// <inheritdoc />
    public partial class changeStockFieldName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Close",
                table: "Stocks",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Stocks",
                newName: "Close");
        }
    }
}
