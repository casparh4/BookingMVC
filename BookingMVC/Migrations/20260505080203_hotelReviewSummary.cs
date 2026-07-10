using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingMVC.Migrations
{
    /// <inheritdoc />
    public partial class hotelReviewSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReviewSummary",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReviewSummary",
                table: "Hotels");
        }
    }
}
