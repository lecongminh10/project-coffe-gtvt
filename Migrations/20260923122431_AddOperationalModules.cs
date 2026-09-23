using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaiTapLon.Migrations
{
    /// <inheritdoc />
    public partial class AddOperationalModules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_UserId_ProductId",
                table: "Wishlists",
                columns: new[] { "UserId", "ProductId" },
                unique: true);

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_UserId",
                table: "Wishlists");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ProductId_IngredientId",
                table: "Recipes",
                columns: new[] { "ProductId", "IngredientId" },
                unique: true);

            migrationBuilder.DropIndex(
                name: "IX_Recipes_ProductId",
                table: "Recipes");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSchedules_UserId_ShiftId_WorkDate",
                table: "EmployeeSchedules",
                columns: new[] { "UserId", "ShiftId", "WorkDate" },
                unique: true);

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSchedules_UserId",
                table: "EmployeeSchedules");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountAmount",
                table: "Orders",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "InventoryDeducted",
                table: "Orders",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VoucherCode",
                table: "Orders",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "BaseUnit",
                table: "Ingredients",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "g")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "BaseUnitsPerStockUnit",
                table: "Ingredients",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 1m);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckInTime",
                table: "EmployeeSchedules",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckOutTime",
                table: "EmployeeSchedules",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StockTransactions",
                columns: table => new
                {
                    StockTransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BalanceAfter = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.StockTransactionId);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredients",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "EmployeeSchedules",
                keyColumn: "ScheduleId",
                keyValue: 1,
                columns: new[] { "CheckInTime", "CheckOutTime" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "EmployeeSchedules",
                keyColumn: "ScheduleId",
                keyValue: 2,
                columns: new[] { "CheckInTime", "CheckOutTime" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "EmployeeSchedules",
                keyColumn: "ScheduleId",
                keyValue: 3,
                columns: new[] { "CheckInTime", "CheckOutTime" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 1,
                columns: new[] { "BaseUnit", "BaseUnitsPerStockUnit" },
                values: new object[] { "g", 1000m });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 2,
                columns: new[] { "BaseUnit", "BaseUnitsPerStockUnit" },
                values: new object[] { "g", 1000m });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 3,
                columns: new[] { "BaseUnit", "BaseUnitsPerStockUnit" },
                values: new object[] { "g", 380m });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 4,
                columns: new[] { "BaseUnit", "BaseUnitsPerStockUnit" },
                values: new object[] { "ml", 1000m });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 5,
                columns: new[] { "BaseUnit", "BaseUnitsPerStockUnit" },
                values: new object[] { "g", 1000m });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 6,
                columns: new[] { "BaseUnit", "BaseUnitsPerStockUnit" },
                values: new object[] { "g", 820m });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 7,
                columns: new[] { "BaseUnit", "BaseUnitsPerStockUnit" },
                values: new object[] { "g", 1000m });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 8,
                columns: new[] { "BaseUnit", "BaseUnitsPerStockUnit" },
                values: new object[] { "ml", 750m });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 9,
                columns: new[] { "BaseUnit", "BaseUnitsPerStockUnit" },
                values: new object[] { "g", 1000m });

            migrationBuilder.UpdateData(
                table: "Ingredients",
                keyColumn: "IngredientId",
                keyValue: 10,
                columns: new[] { "BaseUnit", "BaseUnitsPerStockUnit" },
                values: new object[] { "cái", 1m });

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_IngredientId",
                table: "StockTransactions",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_OrderId",
                table: "StockTransactions",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_UserId",
                table: "Wishlists",
                column: "UserId");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_UserId_ProductId",
                table: "Wishlists");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_ProductId",
                table: "Recipes",
                column: "ProductId");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_ProductId_IngredientId",
                table: "Recipes");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSchedules_UserId",
                table: "EmployeeSchedules",
                column: "UserId");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSchedules_UserId_ShiftId_WorkDate",
                table: "EmployeeSchedules");

            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "InventoryDeducted",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "VoucherCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BaseUnit",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "BaseUnitsPerStockUnit",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "CheckInTime",
                table: "EmployeeSchedules");

            migrationBuilder.DropColumn(
                name: "CheckOutTime",
                table: "EmployeeSchedules");

        }
    }
}
