using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Holdy.Migrations
{
    /// <inheritdoc />
    public partial class isProtected_attribute_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProtectedAttribute",
                table: "AccountAttributes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProtectedAttribute",
                table: "AccountAttributes");
        }
    }
}
