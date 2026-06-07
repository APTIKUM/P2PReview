using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2PReview.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addusertags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TechStack",
                table: "ReviewRequests");

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "TechStack",
                table: "ReviewRequests",
                type: "TEXT",
                nullable: true);
        }
    }
}
