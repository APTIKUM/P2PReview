using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2PReview.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addfileidcomment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileId",
                table: "ReviewResponseComments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileId",
                table: "ReviewResponseComments");
        }
    }
}
