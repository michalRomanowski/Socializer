using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Socializer.Database.Migrations.SocializerDb
{
    /// <inheritdoc />
    public partial class PreferenceUrlToDBPediaResource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Preferences",
                newName: "DBPediaResource");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DBPediaResource",
                table: "Preferences",
                newName: "Url");
        }
    }
}
