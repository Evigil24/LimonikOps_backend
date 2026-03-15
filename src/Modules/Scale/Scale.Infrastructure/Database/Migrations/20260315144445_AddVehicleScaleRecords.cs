using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LimonikOne.Modules.Scale.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleScaleRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "vehicle_scale_records",
                schema: "scale",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type_id = table.Column<int>(type: "integer", nullable: false),
                    status_id = table.Column<int>(type: "integer", nullable: false),
                    started_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    closed_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: true
                    ),
                    first_weight = table.Column<decimal>(
                        type: "numeric(18,4)",
                        precision: 18,
                        scale: 4,
                        nullable: true
                    ),
                    first_weight_id = table.Column<Guid>(type: "uuid", nullable: true),
                    second_weight = table.Column<decimal>(
                        type: "numeric(18,4)",
                        precision: 18,
                        scale: 4,
                        nullable: true
                    ),
                    second_weight_id = table.Column<Guid>(type: "uuid", nullable: true),
                    net_weight = table.Column<decimal>(
                        type: "numeric(18,4)",
                        precision: 18,
                        scale: 4,
                        nullable: true
                    ),
                    created_at = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    created_by = table.Column<string>(
                        type: "character varying(100)",
                        maxLength: 100,
                        nullable: false
                    ),
                    DisplayId = table.Column<long>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_scale_records", x => x.id);
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "vehicle_scale_records", schema: "scale");
        }
    }
}
