using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2PReview.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class useravatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarId",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "AspNetUsers");
        }
    }
}
