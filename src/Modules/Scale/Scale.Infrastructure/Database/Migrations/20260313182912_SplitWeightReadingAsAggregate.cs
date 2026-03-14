using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LimonikOne.Modules.Scale.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class SplitWeightReadingAsAggregate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder
                .AddColumn<long>(
                    name: "display_id",
                    schema: "scale",
                    table: "weight_readings",
                    type: "bigint",
                    nullable: false,
                    defaultValue: 0L
                )
                .Annotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityAlwaysColumn
                );

            migrationBuilder.CreateIndex(
                name: "IX_weight_readings_display_id",
                schema: "scale",
                table: "weight_readings",
                column: "display_id",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_weight_readings_display_id",
                schema: "scale",
                table: "weight_readings"
            );

            migrationBuilder.DropColumn(
                name: "display_id",
                schema: "scale",
                table: "weight_readings"
            );
        }
    }
}
