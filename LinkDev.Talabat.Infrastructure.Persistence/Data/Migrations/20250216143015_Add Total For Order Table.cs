using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinkDev.Talabat.Infrastructure.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalForOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Orders",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "Orders");
        }
    }
}
