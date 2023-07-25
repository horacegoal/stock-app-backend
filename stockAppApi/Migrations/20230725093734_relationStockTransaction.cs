using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace stockAppApi.Migrations
{
    /// <inheritdoc />
    public partial class relationStockTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StockId",
                table: "Transaction",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_StockId",
                table: "Transaction",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Stocks_StockId",
                table: "Transaction",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Stocks_StockId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_StockId",
                table: "Transaction");

            migrationBuilder.AlterColumn<int>(
                name: "StockId",
                table: "Transaction",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
