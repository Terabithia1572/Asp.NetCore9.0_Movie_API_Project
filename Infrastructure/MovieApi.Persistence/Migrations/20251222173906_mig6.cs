using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieApi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    SeriesID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeriesTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeriesCoverImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeriesRating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SeriesDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstAirDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SeriesCreatedYear = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeriesAverageEpisodeDuration = table.Column<int>(type: "int", nullable: true),
                    SeriesSeasonCount = table.Column<int>(type: "int", nullable: false),
                    SeriesEpisodeCount = table.Column<int>(type: "int", nullable: false),
                    SeriesStatus = table.Column<bool>(type: "bit", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.SeriesID);
                    table.ForeignKey(
                        name: "FK_Series_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Series_CategoryID",
                table: "Series",
                column: "CategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Series");
        }
    }
}
