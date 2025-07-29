using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Socializer.Database.Migrations.SocializerDb
{
    /// <inheritdoc />
    public partial class ChatHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ConnectionId",
                table: "Chats",
                newName: "ChatHash");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_ConnectionId",
                table: "Chats",
                newName: "IX_Chats_ChatHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChatHash",
                table: "Chats",
                newName: "ConnectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_ChatHash",
                table: "Chats",
                newName: "IX_Chats_ConnectionId");
        }
    }
}
