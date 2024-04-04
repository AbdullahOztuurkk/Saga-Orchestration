using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SagaStateMachineWorkerService.Migrations;

/// <inheritdoc />
public partial class initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "OrderStateInstance",
            columns: table => new
            {
                CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                CurrentState = table.Column<string>(type: "nvarchar(max)", nullable: false),
                BuyerId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                OrderId = table.Column<long>(type: "bigint", nullable: false),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                CardOwnerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CVV = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CardExpireMonth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CardExpireYear = table.Column<string>(type: "nvarchar(max)", nullable: false),
                TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_OrderStateInstance", x => x.CorrelationId);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "OrderStateInstance");
    }
}
