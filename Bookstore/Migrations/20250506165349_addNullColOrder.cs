using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookstore.Migrations
{
    /// <inheritdoc />
    public partial class addNullColOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_HistoryOrders_IdHistoryOrders",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "IdHistoryOrders",
                table: "Orders",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_HistoryOrders_IdHistoryOrders",
                table: "Orders",
                column: "IdHistoryOrders",
                principalTable: "HistoryOrders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_HistoryOrders_IdHistoryOrders",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "IdHistoryOrders",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_HistoryOrders_IdHistoryOrders",
                table: "Orders",
                column: "IdHistoryOrders",
                principalTable: "HistoryOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
