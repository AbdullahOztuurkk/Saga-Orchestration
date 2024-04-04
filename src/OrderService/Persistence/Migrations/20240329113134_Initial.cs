using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Orders",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                BuyerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Address_Line = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Address_Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Address_District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                ResponseMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreateUserId = table.Column<long>(type: "bigint", nullable: false),
                UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeleteUserId = table.Column<long>(type: "bigint", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Orders", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "OrderItems",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProductId = table.Column<int>(type: "int", nullable: false),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                OrderId = table.Column<long>(type: "bigint", nullable: false),
                Count = table.Column<int>(type: "int", nullable: false),
                Status = table.Column<int>(type: "int", nullable: false),
                CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                CreateUserId = table.Column<long>(type: "bigint", nullable: false),
                UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                DeleteUserId = table.Column<long>(type: "bigint", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OrderItems", x => x.Id);
                table.ForeignKey(
                    name: "FK_OrderItems_Orders_OrderId",
                    column: x => x.OrderId,
                    principalTable: "Orders",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_OrderItems_OrderId",
            table: "OrderItems",
            column: "OrderId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "OrderItems");

        migrationBuilder.DropTable(
            name: "Orders");
    }
}
