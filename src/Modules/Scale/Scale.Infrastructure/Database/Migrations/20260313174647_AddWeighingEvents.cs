using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LimonikOne.Modules.Scale.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddWeighingEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "weighing_events",
                schema: "scale",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    device_id = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    location = table.Column<string>(
                        type: "character varying(200)",
                        maxLength: 200,
                        nullable: false
                    ),
                    status = table.Column<string>(
                        type: "character varying(20)",
                        maxLength: 20,
                        nullable: false
                    ),
                    started_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    ended_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    peak_weight = table.Column<decimal>(
                        type: "numeric(18,4)",
                        precision: 18,
                        scale: 4,
                        nullable: false
                    ),
                    display_id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityAlwaysColumn
                        ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weighing_events", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "weighing_measurements",
                schema: "scale",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    weight = table.Column<decimal>(
                        type: "numeric(18,4)",
                        precision: 18,
                        scale: 4,
                        nullable: false
                    ),
                    timestamp = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    stable_count = table.Column<int>(type: "integer", nullable: false),
                    source_reading_id = table.Column<Guid>(type: "uuid", nullable: false),
                    weighing_event_id = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weighing_measurements", x => x.id);
                    table.ForeignKey(
                        name: "FK_weighing_measurements_weighing_events_weighing_event_id",
                        column: x => x.weighing_event_id,
                        principalSchema: "scale",
                        principalTable: "weighing_events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_weighing_events_device_id_status",
                schema: "scale",
                table: "weighing_events",
                columns: new[] { "device_id", "status" }
            );

            migrationBuilder.CreateIndex(
                name: "IX_weighing_events_display_id",
                schema: "scale",
                table: "weighing_events",
                column: "display_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_weighing_measurements_weighing_event_id",
                schema: "scale",
                table: "weighing_measurements",
                column: "weighing_event_id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "weighing_measurements", schema: "scale");

            migrationBuilder.DropTable(name: "weighing_events", schema: "scale");
        }
    }
}
