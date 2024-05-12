using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace listing_backend.Migrations
{
    /// <inheritdoc />
    public partial class ListingUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Models_ModelId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Engines_Fuels_FuelId",
                table: "Engines");

            migrationBuilder.DropForeignKey(
                name: "FK_Engines_Makes_MakeId",
                table: "Engines");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Cars_CarId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Categories_CategoryId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Colors_ExteriorColorId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Colors_InteriorColorId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_DoorTypes_DoorTypeId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Engines_EngineId",
                table: "Listings");

            migrationBuilder.AlterColumn<int>(
                name: "InteriorColorId",
                table: "Listings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ExteriorColorId",
                table: "Listings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EngineId",
                table: "Listings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DoorTypeId",
                table: "Listings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Listings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CarId",
                table: "Listings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TractionId",
                table: "Listings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransmissionId",
                table: "Listings",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MakeId",
                table: "Engines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FuelId",
                table: "Engines",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ModelId",
                table: "Cars",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_TractionId",
                table: "Listings",
                column: "TractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Listings_TransmissionId",
                table: "Listings",
                column: "TransmissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Models_ModelId",
                table: "Cars",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Engines_Fuels_FuelId",
                table: "Engines",
                column: "FuelId",
                principalTable: "Fuels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Engines_Makes_MakeId",
                table: "Engines",
                column: "MakeId",
                principalTable: "Makes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Cars_CarId",
                table: "Listings",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Categories_CategoryId",
                table: "Listings",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Colors_ExteriorColorId",
                table: "Listings",
                column: "ExteriorColorId",
                principalTable: "Colors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Colors_InteriorColorId",
                table: "Listings",
                column: "InteriorColorId",
                principalTable: "Colors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_DoorTypes_DoorTypeId",
                table: "Listings",
                column: "DoorTypeId",
                principalTable: "DoorTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Engines_EngineId",
                table: "Listings",
                column: "EngineId",
                principalTable: "Engines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Tractions_TractionId",
                table: "Listings",
                column: "TractionId",
                principalTable: "Tractions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Transmissions_TransmissionId",
                table: "Listings",
                column: "TransmissionId",
                principalTable: "Transmissions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Models_ModelId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Engines_Fuels_FuelId",
                table: "Engines");

            migrationBuilder.DropForeignKey(
                name: "FK_Engines_Makes_MakeId",
                table: "Engines");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Cars_CarId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Categories_CategoryId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Colors_ExteriorColorId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Colors_InteriorColorId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_DoorTypes_DoorTypeId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Engines_EngineId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Tractions_TractionId",
                table: "Listings");

            migrationBuilder.DropForeignKey(
                name: "FK_Listings_Transmissions_TransmissionId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_TractionId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_TransmissionId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "TractionId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "TransmissionId",
                table: "Listings");

            migrationBuilder.AlterColumn<int>(
                name: "InteriorColorId",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ExteriorColorId",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EngineId",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DoorTypeId",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CarId",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MakeId",
                table: "Engines",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FuelId",
                table: "Engines",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModelId",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Models_ModelId",
                table: "Cars",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Engines_Fuels_FuelId",
                table: "Engines",
                column: "FuelId",
                principalTable: "Fuels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Engines_Makes_MakeId",
                table: "Engines",
                column: "MakeId",
                principalTable: "Makes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Cars_CarId",
                table: "Listings",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Categories_CategoryId",
                table: "Listings",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Colors_ExteriorColorId",
                table: "Listings",
                column: "ExteriorColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Colors_InteriorColorId",
                table: "Listings",
                column: "InteriorColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_DoorTypes_DoorTypeId",
                table: "Listings",
                column: "DoorTypeId",
                principalTable: "DoorTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_Engines_EngineId",
                table: "Listings",
                column: "EngineId",
                principalTable: "Engines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
