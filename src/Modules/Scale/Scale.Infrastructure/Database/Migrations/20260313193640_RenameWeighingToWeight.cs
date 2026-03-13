using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LimonikOne.Modules.Scale.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameWeighingToWeight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "weighing_measurements",
                schema: "scale",
                newName: "weight_measurements",
                newSchema: "scale"
            );

            migrationBuilder.RenameTable(
                name: "weighing_events",
                schema: "scale",
                newName: "weight_events",
                newSchema: "scale"
            );

            migrationBuilder.RenameColumn(
                name: "weighing_event_id",
                schema: "scale",
                table: "weight_measurements",
                newName: "weight_event_id"
            );

            migrationBuilder.RenameIndex(
                name: "IX_weighing_measurements_weighing_event_id",
                schema: "scale",
                table: "weight_measurements",
                newName: "IX_weight_measurements_weight_event_id"
            );

            migrationBuilder.RenameIndex(
                name: "IX_weighing_events_display_id",
                schema: "scale",
                table: "weight_events",
                newName: "IX_weight_events_display_id"
            );

            migrationBuilder.RenameIndex(
                name: "IX_weighing_events_device_id_status",
                schema: "scale",
                table: "weight_events",
                newName: "IX_weight_events_device_id_status"
            );

            migrationBuilder.Sql(
                "ALTER TABLE scale.weight_measurements RENAME CONSTRAINT \"PK_weighing_measurements\" TO \"PK_weight_measurements\""
            );

            migrationBuilder.Sql(
                "ALTER TABLE scale.weight_events RENAME CONSTRAINT \"PK_weighing_events\" TO \"PK_weight_events\""
            );

            migrationBuilder.Sql(
                "ALTER TABLE scale.weight_measurements RENAME CONSTRAINT \"FK_weighing_measurements_weighing_events_weighing_event_id\" TO \"FK_weight_measurements_weight_events_weight_event_id\""
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE scale.weight_measurements RENAME CONSTRAINT \"FK_weight_measurements_weight_events_weight_event_id\" TO \"FK_weighing_measurements_weighing_events_weighing_event_id\""
            );

            migrationBuilder.Sql(
                "ALTER TABLE scale.weight_events RENAME CONSTRAINT \"PK_weight_events\" TO \"PK_weighing_events\""
            );

            migrationBuilder.Sql(
                "ALTER TABLE scale.weight_measurements RENAME CONSTRAINT \"PK_weight_measurements\" TO \"PK_weighing_measurements\""
            );

            migrationBuilder.RenameIndex(
                name: "IX_weight_events_device_id_status",
                schema: "scale",
                table: "weight_events",
                newName: "IX_weighing_events_device_id_status"
            );

            migrationBuilder.RenameIndex(
                name: "IX_weight_events_display_id",
                schema: "scale",
                table: "weight_events",
                newName: "IX_weighing_events_display_id"
            );

            migrationBuilder.RenameIndex(
                name: "IX_weight_measurements_weight_event_id",
                schema: "scale",
                table: "weight_measurements",
                newName: "IX_weighing_measurements_weighing_event_id"
            );

            migrationBuilder.RenameColumn(
                name: "weight_event_id",
                schema: "scale",
                table: "weight_measurements",
                newName: "weighing_event_id"
            );

            migrationBuilder.RenameTable(
                name: "weight_events",
                schema: "scale",
                newName: "weighing_events",
                newSchema: "scale"
            );

            migrationBuilder.RenameTable(
                name: "weight_measurements",
                schema: "scale",
                newName: "weighing_measurements",
                newSchema: "scale"
            );
        }
    }
}
