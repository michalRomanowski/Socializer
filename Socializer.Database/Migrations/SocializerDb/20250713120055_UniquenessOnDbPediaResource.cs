using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Socializer.Database.Migrations.SocializerDb
{
    /// <inheritdoc />
    public partial class UniquenessOnDbPediaResource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Preferences_DBPediaResource",
                table: "Preferences");

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_DBPediaResource",
                table: "Preferences",
                column: "DBPediaResource",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Preferences_DBPediaResource",
                table: "Preferences");

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_DBPediaResource",
                table: "Preferences",
                column: "DBPediaResource");
        }
    }
}
