using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FXTransfer.Migrations
{
    /// <inheritdoc />
    public partial class AddPremiumDiscountToFeeConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PremiumDiscount",
                table: "FeeConfigurations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PremiumDiscount",
                table: "FeeConfigurations");
        }
    }
}
