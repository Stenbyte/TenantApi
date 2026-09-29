using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantApi.Migrations
{
    /// <inheritdoc />
    public partial class AddBuildingSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "building_settings",
                columns: table => new
                {
                    building_id = table.Column<Guid>(type: "uuid", nullable: false),
                    max_bookings_per_week = table.Column<int>(type: "integer", nullable: false, defaultValue: 3),
                    slot_length_minutes = table.Column<int>(type: "integer", nullable: false, defaultValue: 180)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_building_settings", x => x.building_id);
                    table.ForeignKey(
                        name: "FK_building_settings_buildings_building_id",
                        column: x => x.building_id,
                        principalTable: "buildings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "building_settings");
        }
    }
}
