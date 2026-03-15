using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LimonikOne.Modules.Person.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddVendorClassificationAggregate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "classification_id",
                schema: "person",
                table: "vendors"
            );

            migrationBuilder.AddColumn<Guid>(
                name: "classification_id",
                schema: "person",
                table: "vendors",
                type: "uuid",
                nullable: false
            );

            migrationBuilder.CreateTable(
                name: "vendor_classifications",
                schema: "person",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(
                        type: "character varying(300)",
                        maxLength: 300,
                        nullable: false
                    ),
                    description = table.Column<string>(
                        type: "character varying(1000)",
                        maxLength: 1000,
                        nullable: true
                    ),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    display_id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityAlwaysColumn
                        ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendor_classifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_vendor_classifications_vendor_classifications_parent_id",
                        column: x => x.parent_id,
                        principalSchema: "person",
                        principalTable: "vendor_classifications",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_vendors_classification_id",
                schema: "person",
                table: "vendors",
                column: "classification_id"
            );

            migrationBuilder.CreateIndex(
                name: "IX_vendor_classifications_display_id",
                schema: "person",
                table: "vendor_classifications",
                column: "display_id",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_vendor_classifications_parent_id",
                schema: "person",
                table: "vendor_classifications",
                column: "parent_id"
            );

            migrationBuilder.AddForeignKey(
                name: "FK_vendors_vendor_classifications_classification_id",
                schema: "person",
                table: "vendors",
                column: "classification_id",
                principalSchema: "person",
                principalTable: "vendor_classifications",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vendors_vendor_classifications_classification_id",
                schema: "person",
                table: "vendors"
            );

            migrationBuilder.DropTable(name: "vendor_classifications", schema: "person");

            migrationBuilder.DropIndex(
                name: "IX_vendors_classification_id",
                schema: "person",
                table: "vendors"
            );

            migrationBuilder.AlterColumn<int>(
                name: "classification_id",
                schema: "person",
                table: "vendors",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid"
            );
        }
    }
}
