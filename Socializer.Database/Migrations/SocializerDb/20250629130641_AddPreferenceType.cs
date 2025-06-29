using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Socializer.Database.Migrations.SocializerDb
{
    /// <inheritdoc />
    public partial class AddPreferenceType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PreferenceType",
                table: "Preferences",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferenceType",
                table: "Preferences");
        }
    }
}
