using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LimonikOne.Modules.Product.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangeValueObjectColumnsFromNameToId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Cast existing integer-as-string values to integer type
            migrationBuilder.Sql(
                """
                ALTER TABLE product.products
                    ALTER COLUMN variety TYPE integer USING variety::integer;
                """
            );

            migrationBuilder.Sql(
                """
                ALTER TABLE product.products
                    ALTER COLUMN handling TYPE integer USING handling::integer;
                """
            );

            migrationBuilder.Sql(
                """
                ALTER TABLE product.products
                    ALTER COLUMN certification TYPE integer USING certification::integer;
                """
            );

            migrationBuilder.Sql(
                """
                ALTER TABLE product.products
                    ALTER COLUMN stage TYPE integer USING stage::integer;
                """
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "variety",
                schema: "product",
                table: "products",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );

            migrationBuilder.AlterColumn<string>(
                name: "stage",
                schema: "product",
                table: "products",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );

            migrationBuilder.AlterColumn<string>(
                name: "handling",
                schema: "product",
                table: "products",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );

            migrationBuilder.AlterColumn<string>(
                name: "certification",
                schema: "product",
                table: "products",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer"
            );
        }
    }
}
