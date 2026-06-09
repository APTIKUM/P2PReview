using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2PReview.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addautogetreviewuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAutoGetReview",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAutoGetReview",
                table: "AspNetUsers");
        }
    }
}
