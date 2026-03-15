using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LimonikOne.Modules.Product.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameProductsTableToItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "products",
                schema: "product",
                newName: "items",
                newSchema: "product"
            );

            migrationBuilder.RenameIndex(
                name: "IX_products_display_id",
                schema: "product",
                table: "items",
                newName: "IX_items_display_id"
            );

            migrationBuilder.RenameIndex(
                name: "IX_products_item_number",
                schema: "product",
                table: "items",
                newName: "IX_items_item_number"
            );

            migrationBuilder.RenameIndex(
                name: "IX_products_search_name",
                schema: "product",
                table: "items",
                newName: "IX_items_search_name"
            );

            migrationBuilder.Sql(
                "ALTER TABLE product.items RENAME CONSTRAINT \"PK_products\" TO \"PK_items\""
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE product.items RENAME CONSTRAINT \"PK_items\" TO \"PK_products\""
            );

            migrationBuilder.RenameIndex(
                name: "IX_items_search_name",
                schema: "product",
                table: "items",
                newName: "IX_products_search_name"
            );

            migrationBuilder.RenameIndex(
                name: "IX_items_item_number",
                schema: "product",
                table: "items",
                newName: "IX_products_item_number"
            );

            migrationBuilder.RenameIndex(
                name: "IX_items_display_id",
                schema: "product",
                table: "items",
                newName: "IX_products_display_id"
            );

            migrationBuilder.RenameTable(
                name: "items",
                schema: "product",
                newName: "products",
                newSchema: "product"
            );
        }
    }
}
