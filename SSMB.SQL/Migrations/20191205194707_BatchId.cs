using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMB.SQL.Migrations
{
    public partial class BatchId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderBatchBatchId",
                table: "LtsOrder",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderBatchBatchId1",
                table: "LtsOrder",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LtsOrder_OrderBatchBatchId",
                table: "LtsOrder",
                column: "OrderBatchBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_LtsOrder_OrderBatchBatchId1",
                table: "LtsOrder",
                column: "OrderBatchBatchId1");

            /*migrationBuilder.AddForeignKey(
                name: "FK_LtsOrder_OrderBatch_OrderBatchBatchId",
                table: "LtsOrder",
                column: "OrderBatchBatchId",
                principalTable: "OrderBatch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LtsOrder_OrderBatch_OrderBatchBatchId1",
                table: "LtsOrder",
                column: "OrderBatchBatchId1",
                principalTable: "OrderBatch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);*/
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.DropForeignKey(
                name: "FK_LtsOrder_OrderBatch_OrderBatchBatchId",
                table: "LtsOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_LtsOrder_OrderBatch_OrderBatchBatchId1",
                table: "LtsOrder");*/

            migrationBuilder.DropIndex(
                name: "IX_LtsOrder_OrderBatchBatchId",
                table: "LtsOrder");

            migrationBuilder.DropIndex(
                name: "IX_LtsOrder_OrderBatchBatchId1",
                table: "LtsOrder");

            migrationBuilder.DropColumn(
                name: "OrderBatchBatchId",
                table: "LtsOrder");

            migrationBuilder.DropColumn(
                name: "OrderBatchBatchId1",
                table: "LtsOrder");
        }
    }
}
