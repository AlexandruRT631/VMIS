using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace listing_backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DoorTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorTypes", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Fuels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fuels", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Makes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Makes", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tractions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tractions", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Transmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transmissions", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Engines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MakeId = table.Column<int>(type: "int", nullable: false),
                    EngineCode = table.Column<string>(type: "longtext", nullable: false),
                    Displacement = table.Column<int>(type: "int", nullable: false),
                    FuelId = table.Column<int>(type: "int", nullable: false),
                    Power = table.Column<int>(type: "int", nullable: false),
                    Torque = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Engines_Fuels_FuelId",
                        column: x => x.FuelId,
                        principalTable: "Fuels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Engines_Makes_MakeId",
                        column: x => x.MakeId,
                        principalTable: "Makes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MakeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    EndYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_Makes_MakeId",
                        column: x => x.MakeId,
                        principalTable: "Makes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CategoryModel",
                columns: table => new
                {
                    PossibleCategoriesId = table.Column<int>(type: "int", nullable: false),
                    PossibleModelsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryModel", x => new { x.PossibleCategoriesId, x.PossibleModelsId });
                    table.ForeignKey(
                        name: "FK_CategoryModel_Categories_PossibleCategoriesId",
                        column: x => x.PossibleCategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryModel_Models_PossibleModelsId",
                        column: x => x.PossibleModelsId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DoorTypeModel",
                columns: table => new
                {
                    PossibleDoorTypesId = table.Column<int>(type: "int", nullable: false),
                    PossibleModelsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoorTypeModel", x => new { x.PossibleDoorTypesId, x.PossibleModelsId });
                    table.ForeignKey(
                        name: "FK_DoorTypeModel_DoorTypes_PossibleDoorTypesId",
                        column: x => x.PossibleDoorTypesId,
                        principalTable: "DoorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DoorTypeModel_Models_PossibleModelsId",
                        column: x => x.PossibleModelsId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EngineModel",
                columns: table => new
                {
                    PossibleEnginesId = table.Column<int>(type: "int", nullable: false),
                    PossibleModelsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineModel", x => new { x.PossibleEnginesId, x.PossibleModelsId });
                    table.ForeignKey(
                        name: "FK_EngineModel_Engines_PossibleEnginesId",
                        column: x => x.PossibleEnginesId,
                        principalTable: "Engines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EngineModel_Models_PossibleModelsId",
                        column: x => x.PossibleModelsId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    EngineId = table.Column<int>(type: "int", nullable: false),
                    DoorTypeId = table.Column<int>(type: "int", nullable: false),
                    InteriorColorId = table.Column<int>(type: "int", nullable: false),
                    ExteriorColorId = table.Column<int>(type: "int", nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Listings_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Listings_Colors_ExteriorColorId",
                        column: x => x.ExteriorColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Listings_Colors_InteriorColorId",
                        column: x => x.InteriorColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Listings_DoorTypes_DoorTypeId",
                        column: x => x.DoorTypeId,
                        principalTable: "DoorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Listings_Engines_EngineId",
                        column: x => x.EngineId,
                        principalTable: "Engines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Listings_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ModelTraction",
                columns: table => new
                {
                    PossibleModelsId = table.Column<int>(type: "int", nullable: false),
                    PossibleTractionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelTraction", x => new { x.PossibleModelsId, x.PossibleTractionsId });
                    table.ForeignKey(
                        name: "FK_ModelTraction_Models_PossibleModelsId",
                        column: x => x.PossibleModelsId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelTraction_Tractions_PossibleTractionsId",
                        column: x => x.PossibleTractionsId,
                        principalTable: "Tractions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ModelTransmission",
                columns: table => new
                {
                    PossibleModelsId = table.Column<int>(type: "int", nullable: false),
                    PossibleTransmissionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelTransmission", x => new { x.PossibleModelsId, x.PossibleTransmissionsId });
                    table.ForeignKey(
                        name: "FK_ModelTransmission_Models_PossibleModelsId",
                        column: x => x.PossibleModelsId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelTransmission_Transmissions_PossibleTransmissionsId",
                        column: x => x.PossibleTransmissionsId,
                        principalTable: "Transmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryModel_PossibleModelsId",
                table: "CategoryModel",
                column: "PossibleModelsId");

            migrationBuilder.CreateIndex(
                name: "IX_DoorTypeModel_PossibleModelsId",
                table: "DoorTypeModel",
                column: "PossibleModelsId");

            migrationBuilder.CreateIndex(
                name: "IX_EngineModel_PossibleModelsId",
                table: "EngineModel",
                column: "PossibleModelsId");

            migrationBuilder.CreateIndex(
                name: "IX_Engines_FuelId",
                table: "Engines",
                column: "FuelId");

            migrationBuilder.CreateIndex(
                name: "IX_Engines_MakeId",
                table: "Engines",
                column: "MakeId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_CategoryId",
                table: "Listings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_DoorTypeId",
                table: "Listings",
                column: "DoorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_EngineId",
                table: "Listings",
                column: "EngineId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_ExteriorColorId",
                table: "Listings",
                column: "ExteriorColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_InteriorColorId",
                table: "Listings",
                column: "InteriorColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_ModelId",
                table: "Listings",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_MakeId",
                table: "Models",
                column: "MakeId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelTraction_PossibleTractionsId",
                table: "ModelTraction",
                column: "PossibleTractionsId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelTransmission_PossibleTransmissionsId",
                table: "ModelTransmission",
                column: "PossibleTransmissionsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryModel");

            migrationBuilder.DropTable(
                name: "DoorTypeModel");

            migrationBuilder.DropTable(
                name: "EngineModel");

            migrationBuilder.DropTable(
                name: "Listings");

            migrationBuilder.DropTable(
                name: "ModelTraction");

            migrationBuilder.DropTable(
                name: "ModelTransmission");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "DoorTypes");

            migrationBuilder.DropTable(
                name: "Engines");

            migrationBuilder.DropTable(
                name: "Tractions");

            migrationBuilder.DropTable(
                name: "Models");

            migrationBuilder.DropTable(
                name: "Transmissions");

            migrationBuilder.DropTable(
                name: "Fuels");

            migrationBuilder.DropTable(
                name: "Makes");
        }
    }
}
