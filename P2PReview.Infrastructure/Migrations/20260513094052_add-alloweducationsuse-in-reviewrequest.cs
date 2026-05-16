using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace P2PReview.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addalloweducationsuseinreviewrequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeadLine",
                table: "ReviewRequests",
                newName: "Deadline");

            migrationBuilder.RenameColumn(
                name: "ProgrammingLanguage",
                table: "ReviewRequests",
                newName: "TechStack");

            migrationBuilder.RenameColumn(
                name: "IsForEducation",
                table: "ReviewRequests",
                newName: "ReviewersCount");

            migrationBuilder.AddColumn<bool>(
                name: "AllowEducationalUse",
                table: "ReviewRequests",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowEducationalUse",
                table: "ReviewRequests");

            migrationBuilder.RenameColumn(
                name: "Deadline",
                table: "ReviewRequests",
                newName: "DeadLine");

            migrationBuilder.RenameColumn(
                name: "TechStack",
                table: "ReviewRequests",
                newName: "ProgrammingLanguage");

            migrationBuilder.RenameColumn(
                name: "ReviewersCount",
                table: "ReviewRequests",
                newName: "IsForEducation");
        }
    }
}
