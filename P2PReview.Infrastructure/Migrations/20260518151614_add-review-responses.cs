using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2PReview.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addreviewresponses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReviewResponses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ReviewRequestId = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CodeRating = table.Column<int>(type: "INTEGER", nullable: true),
                    Summary = table.Column<string>(type: "TEXT", nullable: true),
                    ReviewRating = table.Column<int>(type: "INTEGER", nullable: true),
                    ReviewFeedback = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewResponses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReviewResponses_ReviewRequests_ReviewRequestId",
                        column: x => x.ReviewRequestId,
                        principalTable: "ReviewRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewResponseComments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    ReviewResponseId = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Line = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewResponseComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewResponseComments_ReviewResponses_ReviewResponseId",
                        column: x => x.ReviewResponseId,
                        principalTable: "ReviewResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReviewResponseComments_ReviewResponseId",
                table: "ReviewResponseComments",
                column: "ReviewResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewResponses_ReviewRequestId",
                table: "ReviewResponses",
                column: "ReviewRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewResponses_UserId",
                table: "ReviewResponses",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReviewResponseComments");

            migrationBuilder.DropTable(
                name: "ReviewResponses");
        }
    }
}
