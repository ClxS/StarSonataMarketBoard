using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMB.SQL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Cost = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Quality = table.Column<string>(unicode: false, nullable: false),
                    ScrapValue = table.Column<long>(nullable: false, defaultValue: -1L),
                    Space = table.Column<int>(nullable: false),
                    StructuredDescription = table.Column<string>(nullable: false),
                    Type = table.Column<string>(unicode: false, nullable: false),
                    Weight = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderBatch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(nullable: false),
                    ItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderBatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderBatch_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LtsOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OrderType = table.Column<string>(unicode: false, nullable: false),
                    Price = table.Column<long>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    BatchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LtsOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LtsOrder_OrderBatch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "OrderBatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BaseName = table.Column<string>(maxLength: 70, nullable: false),
                    GalaxyName = table.Column<string>(maxLength: 70, nullable: false),
                    IsUserBase = table.Column<bool>(nullable: false),
                    OrderType = table.Column<string>(unicode: false, nullable: false),
                    Price = table.Column<long>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    BatchId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderEntry_OrderBatch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "OrderBatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LtsOrder_BatchId",
                table: "LtsOrder",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderBatch_ItemId",
                table: "OrderBatch",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderEntry_BatchId",
                table: "OrderEntry",
                column: "BatchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LtsOrder");

            migrationBuilder.DropTable(
                name: "OrderEntry");

            migrationBuilder.DropTable(
                name: "OrderBatch");

            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
