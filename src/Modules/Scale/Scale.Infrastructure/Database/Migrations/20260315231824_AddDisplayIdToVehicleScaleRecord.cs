using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LimonikOne.Modules.Scale.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddDisplayIdToVehicleScaleRecord : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DisplayId",
                schema: "scale",
                table: "vehicle_scale_records",
                newName: "display_id"
            );

            migrationBuilder
                .AlterColumn<long>(
                    name: "display_id",
                    schema: "scale",
                    table: "vehicle_scale_records",
                    type: "bigint",
                    nullable: false,
                    oldClrType: typeof(long),
                    oldType: "bigint"
                )
                .Annotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityAlwaysColumn
                );

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_scale_records_display_id",
                schema: "scale",
                table: "vehicle_scale_records",
                column: "display_id",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_vehicle_scale_records_display_id",
                schema: "scale",
                table: "vehicle_scale_records"
            );

            migrationBuilder.RenameColumn(
                name: "display_id",
                schema: "scale",
                table: "vehicle_scale_records",
                newName: "DisplayId"
            );

            migrationBuilder
                .AlterColumn<long>(
                    name: "DisplayId",
                    schema: "scale",
                    table: "vehicle_scale_records",
                    type: "bigint",
                    nullable: false,
                    oldClrType: typeof(long),
                    oldType: "bigint"
                )
                .OldAnnotation(
                    "Npgsql:ValueGenerationStrategy",
                    NpgsqlValueGenerationStrategy.IdentityAlwaysColumn
                );
        }
    }
}
