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
            name: "Stocks",
            columns: table => new
            {
                Id = table.Column<long>(type: "bigint", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ProductId = table.Column<int>(type: "int", nullable: false),
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
                table.PrimaryKey("PK_Stocks", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Stocks");
    }
}
