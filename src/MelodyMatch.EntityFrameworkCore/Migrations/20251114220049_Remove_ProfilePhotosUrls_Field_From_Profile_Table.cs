using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MelodyMatch.Migrations
{
    /// <inheritdoc />
    public partial class Remove_ProfilePhotosUrls_Field_From_Profile_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePhotoUrls",
                schema: "public",
                table: "UserProfiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "ProfilePhotoUrls",
                schema: "public",
                table: "UserProfiles",
                type: "text[]",
                maxLength: 2048,
                nullable: false);
        }
    }
}
