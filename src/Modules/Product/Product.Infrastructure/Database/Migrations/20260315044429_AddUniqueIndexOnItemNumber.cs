using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LimonikOne.Modules.Product.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexOnItemNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_products_item_number",
                schema: "product",
                table: "products");

            migrationBuilder.CreateIndex(
                name: "IX_products_item_number",
                schema: "product",
                table: "products",
                column: "item_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_products_item_number",
                schema: "product",
                table: "products");

            migrationBuilder.CreateIndex(
                name: "IX_products_item_number",
                schema: "product",
                table: "products",
                column: "item_number");
        }
    }
}
