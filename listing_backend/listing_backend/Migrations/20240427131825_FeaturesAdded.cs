using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace listing_backend.Migrations
{
    /// <inheritdoc />
    public partial class FeaturesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeatureExterior",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureExterior", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FeatureInterior",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureInterior", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FeatureExteriorListing",
                columns: table => new
                {
                    FeaturesExteriorId = table.Column<int>(type: "int", nullable: false),
                    ListingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureExteriorListing", x => new { x.FeaturesExteriorId, x.ListingsId });
                    table.ForeignKey(
                        name: "FK_FeatureExteriorListing_FeatureExterior_FeaturesExteriorId",
                        column: x => x.FeaturesExteriorId,
                        principalTable: "FeatureExterior",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureExteriorListing_Listings_ListingsId",
                        column: x => x.ListingsId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FeatureInteriorListing",
                columns: table => new
                {
                    FeaturesInteriorId = table.Column<int>(type: "int", nullable: false),
                    ListingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureInteriorListing", x => new { x.FeaturesInteriorId, x.ListingsId });
                    table.ForeignKey(
                        name: "FK_FeatureInteriorListing_FeatureInterior_FeaturesInteriorId",
                        column: x => x.FeaturesInteriorId,
                        principalTable: "FeatureInterior",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureInteriorListing_Listings_ListingsId",
                        column: x => x.ListingsId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureExteriorListing_ListingsId",
                table: "FeatureExteriorListing",
                column: "ListingsId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureInteriorListing_ListingsId",
                table: "FeatureInteriorListing",
                column: "ListingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureExteriorListing");

            migrationBuilder.DropTable(
                name: "FeatureInteriorListing");

            migrationBuilder.DropTable(
                name: "FeatureExterior");

            migrationBuilder.DropTable(
                name: "FeatureInterior");
        }
    }
}
