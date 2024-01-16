using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BigBasketApplication.Migrations
{
    /// <inheritdoc />
    public partial class PriceChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "price",
                table: "OrderItems",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderItems",
                newName: "price");
        }
    }
}
