using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace listing_backend.Migrations
{
    /// <inheritdoc />
    public partial class ListingTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Listings",
                type: "longtext",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Listings");
        }
    }
}
