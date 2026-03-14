using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LimonikOne.Modules.Scale.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "scale");

            migrationBuilder.CreateTable(
                name: "weight_batches",
                schema: "scale",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_batch_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    sent_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    received_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
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
                    table.PrimaryKey("PK_weight_batches", x => x.id);
                }
            );

            migrationBuilder.CreateTable(
                name: "weight_readings",
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
                    count = table.Column<int>(type: "integer", nullable: false),
                    first_timestamp = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    last_timestamp = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    stable_count = table.Column<int>(type: "integer", nullable: false),
                    weight_batch_id = table.Column<Guid>(type: "uuid", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weight_readings", x => x.id);
                    table.ForeignKey(
                        name: "FK_weight_readings_weight_batches_weight_batch_id",
                        column: x => x.weight_batch_id,
                        principalSchema: "scale",
                        principalTable: "weight_batches",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_weight_batches_display_id",
                schema: "scale",
                table: "weight_batches",
                column: "display_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_weight_batches_external_batch_id",
                schema: "scale",
                table: "weight_batches",
                column: "external_batch_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_weight_readings_weight_batch_id",
                schema: "scale",
                table: "weight_readings",
                column: "weight_batch_id"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "weight_readings", schema: "scale");

            migrationBuilder.DropTable(name: "weight_batches", schema: "scale");
        }
    }
}
