using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Socializer.Database.Migrations.SocializerDb
{
    /// <inheritdoc />
    public partial class RemovePreferenceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Preferences_DBPediaResource",
                table: "Preferences");

            migrationBuilder.DropColumn(
                name: "PreferenceType",
                table: "Preferences");

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_DBPediaResource",
                table: "Preferences",
                column: "DBPediaResource");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Preferences_DBPediaResource",
                table: "Preferences");

            migrationBuilder.AddColumn<int>(
                name: "PreferenceType",
                table: "Preferences",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_DBPediaResource",
                table: "Preferences",
                column: "DBPediaResource",
                unique: true);
        }
    }
}
