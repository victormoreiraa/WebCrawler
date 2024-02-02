using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebCrawler.Migrations
{
    /// <inheritdoc />
    public partial class INITIAL_MIGRATION : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrawledData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PagesCount = table.Column<int>(type: "int", nullable: false),
                    RowsCount = table.Column<int>(type: "int", nullable: false),
                    JsonFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrawledData", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrawledData");
        }
    }
}
