using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MelodyMatch.Migrations
{
    /// <inheritdoc />
    public partial class Add_ShownUserProfiles_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShownUserProfiles",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShownUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShownAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Reacted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShownUserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShownUserProfiles_MelodyMatchUsers_ShownUserId",
                        column: x => x.ShownUserId,
                        principalSchema: "public",
                        principalTable: "MelodyMatchUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShownUserProfiles_MelodyMatchUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "MelodyMatchUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShownUserProfiles_ShownUserId",
                schema: "public",
                table: "ShownUserProfiles",
                column: "ShownUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShownUserProfiles_UserId_ShownUserId",
                schema: "public",
                table: "ShownUserProfiles",
                columns: new[] { "UserId", "ShownUserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShownUserProfiles",
                schema: "public");
        }
    }
}
